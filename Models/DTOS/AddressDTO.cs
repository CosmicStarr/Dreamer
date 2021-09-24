using System.ComponentModel.DataAnnotations;

namespace Models.DTOS
{
    public class AddressDTO
    {
        public int AddressId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }   
    }
}