namespace Aplicatie.API.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }

        public User User { get; set; }
    }
}
