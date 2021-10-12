using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LoginModel
    {  
        [Required(ErrorMessage ="Email is Required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is Required!")]
        public string Password { get; set; }
        public string token {get; set;}
        public bool? RememberMe { get; set; }
    }
}