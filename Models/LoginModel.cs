namespace Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string token {get; set;}
        public bool? RememberMe { get; set; }
    }
}