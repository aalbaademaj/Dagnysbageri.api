using Dagnysbageri.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dagnysbageri.api.Data.Migrations
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesProduct> SalesProducts { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PostalAddress> PostalAddresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<SupplierAddress> SupplierAddresses { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierProduct>().HasKey(s => new { s.ProductId, s.SupplierId });
            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.SalesOrderId, oi.SalesProductId });
            modelBuilder.Entity<CustomerAddress>().HasKey(ca => new { ca.AddressId, ca.CustomerId });
            modelBuilder.Entity<SupplierAddress>().HasKey(sa => new { sa.AddressId, sa.SupplierId });
            base.OnModelCreating(modelBuilder);

        }
    }
}