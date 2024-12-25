using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropagatingKindness.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedDateToAdvert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Adverts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Adverts");
        }
    }
}
