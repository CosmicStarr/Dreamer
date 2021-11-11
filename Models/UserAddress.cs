using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserAddress
    {
        public int UserAddressId  { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; } 
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}