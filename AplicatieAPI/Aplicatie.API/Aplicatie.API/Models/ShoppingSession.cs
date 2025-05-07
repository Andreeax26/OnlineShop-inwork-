using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicatie.API.Models
{
    public class ShoppingSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public virtual List<CartItem> CartItems { get; set; } = new();
    }
}
