using System.ComponentModel.DataAnnotations;
using System;
namespace BeltExamASP.Models
{
    public class PersonViewModel : BaseEntity
    {
// ========================================================================================
// ========================================================================================
        [Key]
        public int id { get; set; }
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="First name field can not be empty")]
        [MinLength(3, ErrorMessage="First name is too Short, must be at least 3 characters long.")]
        public string firstName { get; set; }
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="Last name field can not be empty")]
        [MinLength(3, ErrorMessage="Last name is too Short, must be at least 3 characters long.")]
        public string lastName { get; set; }
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="Username field can not be empty")]
        [MinLength(3, ErrorMessage="Username is too Short, must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage="Username is too long, must be at max 20 characters long.")]
        public string userName { get; set; }
// ========================================================================================
// ========================================================================================
        [Required(ErrorMessage="Password field can not be empty")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
// ========================================================================================
// ========================================================================================
        [Compare("password", ErrorMessage="Passwords don't match")]
        public string confirmPassword {get; set;}
// ========================================================================================
// ========================================================================================

      
        
    }
}
