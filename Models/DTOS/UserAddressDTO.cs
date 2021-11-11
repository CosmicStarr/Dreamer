using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class UserAddressDTO
    {
        public int UserAddressId { get; set; }
        [Required]       
        public string FirstName { get; set; }
        [Required]  
        public string LastName { get; set; }
        [Required]  
        public string  Street { get; set; }
        [Required]  
        public string City { get; set; }
        [Required]  
        public string State { get; set; }
        [Required]  
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }   
    }
}