
using System;
using System.ComponentModel.DataAnnotations;
namespace BeltExamASP.Models
{
    public class AuctionViewModel : BaseEntity {
        [Key]
        public int Id {get; set;}
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="Product Name field can not be empty")]
        [MinLength(3, ErrorMessage="Product name must be at least 3 characters long")]
        public string ProductName {get; set;}
// ========================================================================================
// ========================================================================================

        [Required(ErrorMessage="Description field can not be empty")]
        [MinLength(10, ErrorMessage="Description must be at least 10 chars!")]
        public string Description {get; set;}
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="Starting Bid field can not be empty")]
        [Range(1, Int32.MaxValue, ErrorMessage="Starting Bidmust be at least $1")]
        public int Bid {get; set;}
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="End Date field can not be empty")]
        public string EndDate{get; set;}
// ========================================================================================
// ========================================================================================


    }
}