using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicatie.API.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public PaymentDetails? Payment { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();


    }
}
