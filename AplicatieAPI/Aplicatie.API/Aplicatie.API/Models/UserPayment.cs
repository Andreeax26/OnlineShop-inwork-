namespace Aplicatie.API.Models
{
    public class UserPayment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PaymentType { get; set; }
        public string Provider { get; set; }
        public int AccountNo { get; set; }
        public DateTime Expiry { get; set; }

        public User User { get; set; }
    }
}
