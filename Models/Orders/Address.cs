using System.ComponentModel.DataAnnotations;

namespace Models.Orders
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string street, string state, string city, string zipcode)
        {
            Street = street;
            State = state;
            City = city;
            ZipCode = zipcode;
        }
 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }  
    }
}