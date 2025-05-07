using System;

namespace Aplicatie.API.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
