using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDevMasterClass.Services.Orders.Data.Migrations;

[Migration("002_EventsMigration")]
[DbContext(typeof(OrdersContext))]
public class EventsMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new {
                Id = table.Column<int>()
                    .Annotation("SqlServer:Identity", "1, 1"),
                EventType = table.Column<string>(maxLength: 32),
                Date = table.Column<DateTimeOffset>(),
                Data = table.Column<string>(),
                State = table.Column<string>(),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
            }
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Events");
    }
}