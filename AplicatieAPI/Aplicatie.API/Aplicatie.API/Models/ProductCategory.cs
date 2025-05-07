using System;
using System.Collections.Generic;

namespace Aplicatie.API.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
