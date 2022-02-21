using Microsoft.EntityFrameworkCore.Migrations;

namespace AppFabric.Persistence.Migrations
{
    public partial class Versioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "UsersProjection",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "ProjectsProjection",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UsersProjection");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProjectsProjection");
        }
    }
}
