using System.ComponentModel.DataAnnotations;

namespace Models.DTOS
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
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