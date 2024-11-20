using Microsoft.EntityFrameworkCore;
using WebDevMasterClass.Services.Orders.Entities;

namespace WebDevMasterClass.Services.Orders.Data;

public class OrdersContext : DbContext
{
    public OrdersContext(DbContextOptions<OrdersContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(x =>
            {
                x.ToTable("Orders");

                x.Property<int>("id");

                x.HasMany<OrderItem>("_items").WithOne().HasForeignKey("OrderId").IsRequired();
                x.HasMany<Address>("_addresses").WithOne().HasForeignKey("OrderId").IsRequired();

                x.Ignore(x => x.Items);

                x.HasKey("id");
            });

        modelBuilder.Entity<OrderItem>(x =>
                {
                    x.ToTable("OrderItems");

                    x.Property<int>("id").HasColumnName("ItemId");
                    x.Property<int>("OrderId");

                    x.HasKey("id");
                });

        modelBuilder.Entity<Address>(x =>
        {
            x.ToTable("Addresses");

            x.Property<int>("Id");
            x.Property<int>("OrderId");
            x.Property<string>("AddressType");

            x.HasDiscriminator<string>("AddressType")
                  .HasValue<BillingAddress>("Billing")
                  .HasValue<DeliveryAddress>("Delivery");

            x.HasKey("Id");
        });
    }
}
