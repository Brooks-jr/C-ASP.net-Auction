
using Microsoft.EntityFrameworkCore;
namespace BeltExamASP.Models
{
    public class AuctionContext : DbContext 
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        public DbSet<Person> User {get; set;}
        public DbSet<Auction> Auctions {get; set;}
    }
}