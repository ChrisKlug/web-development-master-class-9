using Microsoft.EntityFrameworkCore;
using WebDevMasterClass.Services.Products.Entities;

namespace WebDevMasterClass.Services.Products.Data;

public class ProductsContext : DbContext
{
    public ProductsContext(DbContextOptions<ProductsContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(x =>
        {
            x.ToTable("Products");
            x.HasKey(x => x.Id);
        });
    }
}
