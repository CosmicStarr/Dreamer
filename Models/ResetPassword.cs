using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage ="Email Address is Required!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Only valid emails are allowed!")]
        [EmailAddressAttribute(ErrorMessage ="Only valid emails are allowed!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}