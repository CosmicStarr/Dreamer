using System.Collections.Generic;
using Models.DTOS.OrderDTO;

namespace Models.DTOS
{
    public class AppUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName;} }
        public ICollection<ActualOrderDTO> Orders { get; set; }
        public AddressDTO Addresses {get; set;}
    }
}