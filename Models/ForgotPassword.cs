using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage ="Email Address is Required!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Only valid emails are allowed!")]
        [EmailAddressAttribute(ErrorMessage ="Only valid emails are allowed!")]
        public string Email { get; set; }
    }
}