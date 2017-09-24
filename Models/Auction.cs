
using System;
using System.Collections.Generic;

namespace BeltExamASP.Models
{
    public class Auction : BaseEntity {
        public int Id {get; set;}
        
        public string CreatedBy {get; set;}
        public string ProductName {get; set;}
        public string Description {get; set;}
        public DateTime EndDate {get; set;}
        public int Bid {get; set;}
        public string HighestBidder {get; set;}
        public DateTime created_at {get; set;}
        public DateTime updated_at { get; set; }


    }
}