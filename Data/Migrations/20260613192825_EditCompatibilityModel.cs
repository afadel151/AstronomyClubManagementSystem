using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class EditCompatibilityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_AccessoryId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_CompatibleWithId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropColumn(
                name: "CompatibilityNote",
                table: "EquipmentCompatibilities");

            migrationBuilder.RenameColumn(
                name: "CompatibleWithId",
                table: "EquipmentCompatibilities",
                newName: "CompatibleWithModelId");

            migrationBuilder.RenameColumn(
                name: "AccessoryId",
                table: "EquipmentCompatibilities",
                newName: "ModelId");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentCompatibilities_CompatibleWithId",
                table: "EquipmentCompatibilities",
                newName: "IX_EquipmentCompatibilities_CompatibleWithModelId");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "EquipmentModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncludedByDefault",
                table: "EquipmentCompatibilities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "EquipmentCompatibilities",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_CompatibleWithModelId",
                table: "EquipmentCompatibilities",
                column: "CompatibleWithModelId",
                principalTable: "EquipmentModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_ModelId",
                table: "EquipmentCompatibilities",
                column: "ModelId",
                principalTable: "EquipmentModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_CompatibleWithModelId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompatibilities_EquipmentModels_ModelId",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "EquipmentModels");

            migrationBuilder.DropColumn(
                name: "IsIncludedByDefault",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "EquipmentCompatibilities");

            migrationBuilder.RenameColumn(
                name: "CompatibleWithModelId",
                table: "EquipmentCompatibilities",
                newName: "CompatibleWithId");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "EquipmentCompatibilities",
                newName: "AccessoryId");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentCompatibilities_CompatibleWithModelId",
                table: "EquipmentCompatibilities",
                newName: "IX_EquipmentCompatibilities_CompatibleWithId");

            migrationBuilder.AddColumn<string>(
                name: "CompatibilityNote",
                table: "EquipmentCompatibilities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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
    }
}
