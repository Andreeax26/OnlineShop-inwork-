using System;

namespace Aplicatie.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public int InventoryId { get; set; }
        public int? DiscountId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? ImageUrl { get; set; }


        public ProductCategory Category { get; set; }
        public ProductInventory Inventory { get; set; }
        public Discount Discount { get; set; }
    }
}
