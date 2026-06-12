using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveCompatibilityToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_Equipments_CompatibleWithId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_Equipments_EquipmentId",
                table: "EquipmentCompatibilities");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "EquipmentCompatibilities",
                newName: "AccessoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_AccessoryId",
                table: "EquipmentCompatibilities",
                column: "AccessoryId",
                principalTable: "EquipmentModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_CompatibleWithId",
                table: "EquipmentCompatibilities",
                column: "CompatibleWithId",
                principalTable: "EquipmentModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_AccessoryId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_CompatibleWithId",
                table: "EquipmentCompatibilities");

            migrationBuilder.RenameColumn(
                name: "AccessoryId",
                table: "EquipmentCompatibilities",
                newName: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_Equipments_CompatibleWithId",
                table: "EquipmentCompatibilities",
                column: "CompatibleWithId",
                principalTable: "Equipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_Equipments_EquipmentId",
                table: "EquipmentCompatibilities",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");
        }
    }
}
