﻿using System;

namespace Aplicatie.API.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual OrderDetails OrderDetails { get; set; }
        public virtual Product Product { get; set; }
    }
}
