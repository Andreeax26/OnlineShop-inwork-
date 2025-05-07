using System;

namespace Aplicatie.API.Models
{
    public class ProductInventory
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
