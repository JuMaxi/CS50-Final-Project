using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropagatingKindness.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddingPropertyCoverImageToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cover_Photo",
                table: "Blogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover_Photo",
                table: "Blogs");
        }
    }
}
