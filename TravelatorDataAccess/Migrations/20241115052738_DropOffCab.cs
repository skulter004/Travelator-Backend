using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelatorDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DropOffCab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DropOff",
                table: "CabBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropOff",
                table: "CabBookings");
        }
    }
}
