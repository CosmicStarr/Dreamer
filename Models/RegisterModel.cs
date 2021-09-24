using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        #nullable enable
        public string? Role { get; set; } 
        #nullable disable
        public string JobDepartment { get; set; }
    
    }
}