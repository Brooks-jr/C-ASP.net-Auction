
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BeltExamASP.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BeltExamASP.Controllers
{
    public class BeltController : Controller
    {

        private AuctionContext context;

        public BeltController(AuctionContext _context)
        {
            context = _context;
        }
// ========================================================================================
// ========================================================================================

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Person> AllUsers = context.User.ToList();
            ViewBag.Users = AllUsers;
            return View();
        }

// ========================================================================================
// ========================================================================================
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
// ========================================================================================
// ========================================================================================
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(PersonViewModel model)
        {
            if(ModelState.IsValid)
            {
                Person newUser = new Person 
                {
                    firstName = model.firstName,
                    lastName = model.lastName,
                    userName = model.userName,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    wallet = 1000
                };
                PasswordHasher<Person> hasher = new PasswordHasher<Person>();
                newUser.password = hasher.HashPassword(newUser, model.password);
                context.Add(newUser);
                context.SaveChanges();
                HttpContext.Session.SetInt32("currentUserId", newUser.id);
                HttpContext.Session.SetString("currentUserName", newUser.firstName);
                return RedirectToAction("Dash");
            }
            else
            {
        
            return View(model);
            }
        }
// ========================================================================================
// ========================================================================================
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
// ========================================================================================
// ========================================================================================
        [HttpPost]
        [Route("Get_User")]
        public IActionResult Get_User(string Username, string Password)
        {
            
            Person loginuser = context.User.SingleOrDefault(user => user.userName == Username);
            List<string> errors = new List<string>();
            if(Password == null)
            {
                errors.Add("Password Field can not be empty");
                ViewBag.Errors = errors;
                return View("Login");
            }
            if(loginuser == null)
            {
                errors.Add("Invalid Username");
                ViewBag.Errors = errors;
                return View("Login");
            } 
            else
            {
                PasswordHasher<Person> hasher = new PasswordHasher<Person>();
                if(hasher.VerifyHashedPassword(loginuser, loginuser.password, Password) != 0)
                {
                    HttpContext.Session.SetInt32("currentUserId", loginuser.id);
                    HttpContext.Session.SetString("currentUserName", loginuser.firstName);
                    return RedirectToAction("Dash");
                } 
                else
                {
                    errors.Add("Invalid Password");
                    ViewBag.Errors = errors;
                    return View("Login");
                }
            }
        }
// ========================================================================================
// ========================================================================================
        [HttpGet]
        [Route("Dash")]
        public IActionResult Dash()
        { DateTime CurrentTime = DateTime.Now;

            int? userId = HttpContext.Session.GetInt32("currentUserId");
            Person currentUser = context.User.SingleOrDefault(user => user.id == userId);

            List<Auction> allAuctions = context.Auctions.OrderBy(auction => auction.EndDate).Where(auction => auction.EndDate > CurrentTime).ToList();

            List<Auction> expiredAuctions = context.Auctions.Where(auction => auction.EndDate <= CurrentTime).ToList();
            foreach(var auction in expiredAuctions)
                {
                    Person highestBidder = context.User.SingleOrDefault(user => user.firstName == auction.HighestBidder);

                    Person createdBy = context.User.SingleOrDefault(user => user.firstName + " " + user.lastName == auction.CreatedBy);
                    highestBidder.wallet -= auction.Bid;
                    createdBy.wallet += auction.Bid;
                }

            context.SaveChanges();

            Dictionary<int, int> remainingTime = new Dictionary<int, int>();
            foreach(var auction in allAuctions)
                {
                    remainingTime[auction.Id] = (auction.EndDate - CurrentTime).Days;
                }
            
            ViewBag.User = currentUser;
            ViewBag.Auctions = allAuctions;
            ViewBag.Remaining = remainingTime;
            return View("Dash");
        }
// ========================================================================================
// ========================================================================================
        [HttpGet]
        [Route("addAuction")]
        public IActionResult AddAuc()
        {
            int? userId = HttpContext.Session.GetInt32("currentUserId");
            Person currentUser = context.User.SingleOrDefault(user => user.id == userId);
            ViewBag.User = currentUser;
            return View("AddAuction");
        }
// ========================================================================================
// ========================================================================================
        [HttpPost]
        [Route("addAuction")]
        public IActionResult AddAuction(AuctionViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("currentUserId");
            
            Person currentUser = context.User.SingleOrDefault(user => user.id == userId);

            if(ModelState.IsValid)
            {
                Auction newAuction = new Auction 
                {
                    ProductName = model.ProductName,
                    CreatedBy = currentUser.firstName + " " + currentUser.lastName,
                    Description = model.Description,
                    Bid = model.Bid,
                    EndDate = DateTime.Parse(model.EndDate),
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                context.Add(newAuction);
                context.SaveChanges();
                return RedirectToAction("Dash");
            } 
            else
            {
                ViewBag.User = currentUser;
                
                return View(model);
            }
            
        }
// ========================================================================================
// ========================================================================================
        [HttpGet]
        [Route("/auction/{id}")]
        public IActionResult ViewAuction(string id)
        {
            int? userId = HttpContext.Session.GetInt32("currentUserId");
            Person currentUser = context.User.SingleOrDefault(user => user.id == userId);
            
            int auctionId = Int32.Parse(id);
            Auction showingAuction = context.Auctions.SingleOrDefault(auction => auction.Id == auctionId);
            ViewBag.Auction = showingAuction;
            ViewBag.User = currentUser;
            return View();
        }
// ========================================================================================
// ========================================================================================
        [HttpPost]
        [Route("/auction/{id}")]
        public IActionResult ViewAuction(string id, string bid)
        {
            List<string> errors = new List<string>();
            int auctionId = Int32.Parse(id);
            Auction showingAuction = context.Auctions.SingleOrDefault(auction => auction.Id == auctionId);
            if(bid != null)
            {
                int bidAmount = Int32.Parse(bid);
                
                int? userId = HttpContext.Session.GetInt32("currentUserId");
                Person currentUser = context.User.SingleOrDefault(user => user.id == userId);

                if(bidAmount <= showingAuction.Bid)
                {
                    errors.Add("Your bid must be greater than standing bid");
                    ViewBag.AmountErrors = errors;
                    ViewBag.Auction = showingAuction;
                    ViewBag.User = currentUser;

                    return View();
                } 
                else if(bidAmount > currentUser.wallet) 
                {
                    errors.Add("Your wallet amount must be greater than standing bid");
                    ViewBag.AmountErrors = errors;
                    ViewBag.Auction = showingAuction;
                    ViewBag.User = currentUser;
                    
                    return View();
                } 
                else 
                {
                    showingAuction.Bid = bidAmount;
                    showingAuction.HighestBidder = currentUser.firstName;
                    currentUser.wallet -= bidAmount;
                    context.SaveChanges();

                    return RedirectToAction("ViewAuction");
                }
            } 
            else 
            {
                errors.Add("Are you backing down?");
                ViewBag.AmountErrors = errors;
                ViewBag.Auction = showingAuction;
                return View();
            }
        }
// ========================================================================================
// ========================================================================================
      
        [HttpGet]
        [Route("/auction/delete/{id}")]
        public IActionResult DeleteAuction(string id)
        {
            int auctionId = Int32.Parse(id);
            Auction deletedAuction = context.Auctions.SingleOrDefault(auction => auction.Id == auctionId);
            context.Auctions.Remove(deletedAuction);
            context.SaveChanges();

            return RedirectToAction("Dash");
        }
// ========================================================================================
// ========================================================================================  
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
// ========================================================================================
// ========================================================================================
    }
}