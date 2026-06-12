using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RelyOnSpecs_MoveAccessoryToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessory",
                table: "EquipmentModels");

            migrationBuilder.DropColumn(
                name: "OpticalDesign",
                table: "EquipmentModels");

            migrationBuilder.AddColumn<bool>(
                name: "IsDedicated",
                table: "EquipmentCompatibilities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Accessory",
                table: "EquipmentCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SpecsType",
                table: "EquipmentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDedicated",
                table: "EquipmentCompatibilities");

            migrationBuilder.DropColumn(
                name: "Accessory",
                table: "EquipmentCategories");

            migrationBuilder.DropColumn(
                name: "SpecsType",
                table: "EquipmentCategories");

            migrationBuilder.AddColumn<bool>(
                name: "Accessory",
                table: "EquipmentModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OpticalDesign",
                table: "EquipmentModels",
                type: "int",
                nullable: true);
        }
    }
}
