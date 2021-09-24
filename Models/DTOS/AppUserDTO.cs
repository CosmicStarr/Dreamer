namespace Models.DTOS
{
    public class AppUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName;} }
        public AddressDTO AddressDTO { get; set; }
    }
}