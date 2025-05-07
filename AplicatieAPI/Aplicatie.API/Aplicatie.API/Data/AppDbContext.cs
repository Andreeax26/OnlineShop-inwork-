using Aplicatie.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Aplicatie.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definește DbSet pentru fiecare model (tabel)
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPayment> UserPayments { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1️⃣ User → UserAddress (One-to-Many, Cascade Delete)
            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2️⃣ User → UserPayment (One-to-Many, Cascade Delete)
            modelBuilder.Entity<UserPayment>()
                .HasOne(up => up.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3️⃣ User → ShoppingSession (One-to-One, Restrict Delete)
            modelBuilder.Entity<ShoppingSession>()
                .HasOne(ss => ss.User)
                .WithOne(u => u.ShoppingSession)
                .HasForeignKey<ShoppingSession>(ss => ss.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4️⃣ User → OrderDetails (One-to-One, Cascade Delete)
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.User)
                .WithOne(u => u.OrderDetail)
                .HasForeignKey<OrderDetails>(od => od.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 5️⃣ ShoppingSession → CartItems (One-to-Many, Cascade Delete)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Session)
                .WithMany(ss => ss.CartItems)
                .HasForeignKey(ci => ci.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6️⃣ Product → ProductCategory (Many-to-One, Restrict Delete)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 7️⃣ Product → Discount (One-to-One, Cascade Delete)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Discount)
                .WithOne()
                .HasForeignKey<Product>(p => p.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            // 8️⃣ Product → ProductInventory (One-to-One, Cascade Delete)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Inventory)
                .WithOne()
                .HasForeignKey<Product>(p => p.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // 9️⃣ OrderDetails → PaymentDetails (One-to-One, Cascade Delete)
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Payment)
                .WithOne(pd => pd.Order)
                .HasForeignKey<OrderDetails>(od => od.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔟 OrderDetails → OrderItems (One-to-Many, Cascade Delete)
            modelBuilder.Entity<OrderItems>()
               .HasOne(oi => oi.OrderDetails)
               .WithMany(od => od.OrderItems)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            // 1️⃣1️⃣ OrderItems → Product (One-to-One, Restrict Delete)
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithOne()
                .HasForeignKey<OrderItems>(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1️⃣2️⃣ CartItem → Product (One-to-One, Cascade Delete)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithOne()
                .HasForeignKey<CartItem>(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
