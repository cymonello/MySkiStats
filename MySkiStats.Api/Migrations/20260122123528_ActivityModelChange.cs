using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySkiStats.Api.Migrations
{
    /// <inheritdoc />
    public partial class ActivityModelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Activities");
        }
    }
}
