using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropagatingKindness.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAccessLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Access_Level",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access_Level",
                table: "Users");
        }
    }
}
