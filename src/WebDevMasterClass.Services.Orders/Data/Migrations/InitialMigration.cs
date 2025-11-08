using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDevMasterClass.Services.Orders.Data.Migrations;

[Migration("001_InitialMigration")]
[DbContext(typeof(OrdersContext))]
public class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new {
                Id = table.Column<int>()
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrderId = table.Column<string>(maxLength: 16),
                OrderDate = table.Column<DateTimeOffset>(),
                Total = table.Column<decimal>()
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.UniqueConstraint("UQ_Orders_OrderId", x => x.OrderId);
            }
        );

        migrationBuilder.CreateTable(
            name: "OrderItems",
            columns: table => new {
                OrderId = table.Column<int>(),
                ItemId = table.Column<int>(),
                Name = table.Column<string>(maxLength: 128),
                Quantity = table.Column<int>(),
                Price = table.Column<decimal>()
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ItemId });
                table.ForeignKey("FK_OrderItems_Orders_OrderId", x => x.OrderId, "Orders", "Id", onDelete: ReferentialAction.Cascade);
            }
        );

        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new {
                Id = table.Column<int>()
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrderId = table.Column<int>(),
                AddressType = table.Column<string>(maxLength: 16),
                Name = table.Column<string>(maxLength: 128),
                Street1 = table.Column<string>(),
                Street2 = table.Column<string>(nullable: true),
                PostalCode = table.Column<string>(),
                City = table.Column<string>(),
                Country = table.Column<string>()
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
                table.ForeignKey("FK_Addresses_Orders_OrderId", x => x.OrderId, "Orders", "Id", onDelete: ReferentialAction.Cascade);
            }
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Addresses");
        migrationBuilder.DropTable("OrderItems");
        migrationBuilder.DropTable("Orders");
    }
}