using System;

namespace BeltExamASP.Models
{
    public class Bidding : BaseEntity {
        public int Id {get; set;}
        public int UserId {get; set;}
        public Person User {get; set;}
        public int AuctionId {get; set;}
        public Auction Auction {get; set;}
        public DateTime created_at {get; set;}
        public DateTime updated_at {get; set;}
        
    }
}