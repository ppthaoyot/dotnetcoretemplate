using Microsoft.EntityFrameworkCore.Migrations;

namespace RPG_Project.Migrations
{
    public partial class BulkCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bulk",
                columns: table => new
                {
                    BulkId = table.Column<int>(nullable: false),
                    BulkName = table.Column<string>(maxLength: 20, nullable: true),
                    BulkCode = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bulk", x => x.BulkId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bulk");
        }
    }
}
