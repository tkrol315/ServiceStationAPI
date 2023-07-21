using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceStationAPI.Migrations
{
    /// <inheritdoc />
    public partial class CarIdChangedToVehicleIdInOrderNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderNotes_Vehicles_CarId",
                table: "OrderNotes");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "OrderNotes",
                newName: "VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderNotes_CarId",
                table: "OrderNotes",
                newName: "IX_OrderNotes_VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderNotes_Vehicles_VehicleId",
                table: "OrderNotes",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderNotes_Vehicles_VehicleId",
                table: "OrderNotes");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "OrderNotes",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderNotes_VehicleId",
                table: "OrderNotes",
                newName: "IX_OrderNotes_CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderNotes_Vehicles_CarId",
                table: "OrderNotes",
                column: "CarId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
