using System.ComponentModel.DataAnnotations;

namespace Models.DTOS
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Email Address is Required!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Only valid emails are allowed!")]
        [EmailAddressAttribute(ErrorMessage ="Only valid emails are allowed!")]
        public string Email { get; set; }
        [Required(ErrorMessage ="First Name is Required!")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", 
        ErrorMessage = "Characters are not allowed.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required!")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", 
        ErrorMessage = "Characters are not allowed.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string JobDepartment { get; set; }
      
     
    }
}