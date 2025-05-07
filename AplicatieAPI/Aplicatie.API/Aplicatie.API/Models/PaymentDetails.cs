using System;

namespace Aplicatie.API.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public OrderDetails Order { get; set; }
    }
}
