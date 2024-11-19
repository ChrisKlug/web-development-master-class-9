using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDevMasterClass.Services.Products.Data.Migrations;

[Migration("001_InitialMigration")]
[DbContext(typeof(ProductsContext))]
public class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128),
                    Description = table.Column<string>(),
                    Price = table.Column<decimal>(),
                    IsFeatured = table.Column<bool>(),
                    ThumbnailUrl = table.Column<string>(),
                    ImageUrl = table.Column<string>(),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                }
            );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Products");
    }
}

