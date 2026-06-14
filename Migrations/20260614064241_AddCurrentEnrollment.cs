using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseCatalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentEnrollment",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentEnrollment",
                table: "Classes");
        }
    }
}
