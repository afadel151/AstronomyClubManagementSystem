using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveFitsHeaderToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessory",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "FitsInstrume",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "FitsTelescop",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Specifications",
                table: "Equipments");

            migrationBuilder.AddColumn<bool>(
                name: "Accessory",
                table: "EquipmentModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FitsInstrume",
                table: "EquipmentModels",
                type: "varchar(68)",
                unicode: false,
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FitsTelescop",
                table: "EquipmentModels",
                type: "varchar(68)",
                unicode: false,
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specifications",
                table: "EquipmentModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessory",
                table: "EquipmentModels");

            migrationBuilder.DropColumn(
                name: "FitsInstrume",
                table: "EquipmentModels");

            migrationBuilder.DropColumn(
                name: "FitsTelescop",
                table: "EquipmentModels");

            migrationBuilder.DropColumn(
                name: "Specifications",
                table: "EquipmentModels");

            migrationBuilder.AddColumn<bool>(
                name: "Accessory",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FitsInstrume",
                table: "Equipments",
                type: "varchar(68)",
                unicode: false,
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FitsTelescop",
                table: "Equipments",
                type: "varchar(68)",
                unicode: false,
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specifications",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
