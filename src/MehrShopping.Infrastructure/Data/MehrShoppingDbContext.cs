using MehrShopping.Domain.Entities;
using MehrShopping.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MehrShopping.Infrastructure.Data
{
    public class MehrShoppingDbContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

        public MehrShoppingDbContext(DbContextOptions options) : base(options)
        {
        }

        protected MehrShoppingDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.Id);

                e.OwnsOne<NationalCode>(c => c.NationalCode, nav =>
                {
                    nav.Property(n => n.Value).HasColumnName(nameof(Customer.NationalCode));
                    nav.HasIndex(n => n.Value).IsUnique();
                });

                e.OwnsOne<Name>(c => c.FirstName, nav =>
                {
                    nav.Property(n => n.Value).HasColumnName(nameof(Customer.FirstName));
                });

                e.OwnsOne<Name>(c => c.LastName, nav =>
                {
                    nav.Property(n => n.Value).HasColumnName(nameof(Customer.LastName));
                });


            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);

                e.OwnsOne<Name>(p => p.Name, nav =>
                {
                    nav.Property(n => n.Value).HasColumnName(nameof(Product.Name));
                });

                e.OwnsOne<Quantity>(p => p.StockQuantity, nav =>
                {
                    nav.Property(q => q.Value).HasColumnName(nameof(Product.StockQuantity));
                });

                e.Property(p => p.RowVersion)
                .IsRowVersion();
            });

            modelBuilder.Entity<Invoice>(e =>
            {
                e.HasKey(e => e.Id);

                e.HasMany(i => i.Items)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId);

                e.HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId);

            });

            modelBuilder.Entity<InvoiceItem>(e =>
            {
                e.HasKey(ii => ii.Id);

                e.OwnsOne(ii => ii.Quantity, nav =>
                {
                    nav.Property(q => q.Value).HasColumnName(nameof(InvoiceItem.Quantity));
                });

                e.HasOne(ii => ii.Product)
                .WithMany(p => p.Items)
                .HasForeignKey(ii => ii.ProductId);

                e.HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId);
            });
        }
    }
}
