using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerEngagementDashboardApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerInteractions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    InteractionType = table.Column<string>(type: "text", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Outcome = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInteractions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInteractions");
        }
    }
}
