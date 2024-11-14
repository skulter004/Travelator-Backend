using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelatorDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CabbookingsEId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "CabRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "CabBookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.CreateIndex(
                name: "IX_CabRequests_EmployeeId",
                table: "CabRequests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CabRequests_Employees_EmployeeId",
                table: "CabRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabRequests_Employees_EmployeeId",
                table: "CabRequests");

            migrationBuilder.DropIndex(
                name: "IX_CabRequests_EmployeeId",
                table: "CabRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "CabRequests");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Time",
                table: "CabBookings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
