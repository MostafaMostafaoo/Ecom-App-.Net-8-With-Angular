namespace Ecom.core.Entity.Order
{
    public class ShippingAddress : BaseEntity<int>
    {

        public ShippingAddress()
        {
            
        }
        public ShippingAddress(string firstName, string lastName, string city, string zipCode, string street, string state)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            ZipCode = zipCode;
            this.street = street;
            this.state = state;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string street { get; set; }
        public string state { get; set; }
    }
}