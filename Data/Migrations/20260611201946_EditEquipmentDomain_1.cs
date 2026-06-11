using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class EditEquipmentDomain_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EQ_Category",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_EQ_CategoryId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "LoanedTo",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "OpticalDesign",
                table: "Equipments");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "Equipments",
                newName: "PurchasePriceUs");

            migrationBuilder.RenameColumn(
                name: "LoanDueDate",
                table: "Equipments",
                newName: "RetiredDate");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Equipments",
                newName: "TotalUsageHours");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Equipments",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "(sysdatetimeoffset())");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Equipments",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "(sysdatetimeoffset())");

            migrationBuilder.AddColumn<bool>(
                name: "Accessory",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RetirementReason",
                table: "Equipments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    ObjectKey = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EQ_Upload",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OpticalDesign = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EQM_Brand",
                        column: x => x.BrandId,
                        principalTable: "EquipmentBrands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EQM_Category",
                        column: x => x.CategoryId,
                        principalTable: "EquipmentCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ModelId",
                table: "Equipments",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "UK_EQUIPMENTBRAND_Name",
                table: "EquipmentBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_EQUIPMENTBRAND_Slug",
                table: "EquipmentBrands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EQ_CategoryId",
                table: "EquipmentModels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentModels_BrandId",
                table: "EquipmentModels",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUploads_EquipmentId",
                table: "EquipmentUploads",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EQ_Model",
                table: "Equipments",
                column: "ModelId",
                principalTable: "EquipmentModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EQ_Model",
                table: "Equipments");

            migrationBuilder.DropTable(
                name: "EquipmentModels");

            migrationBuilder.DropTable(
                name: "EquipmentUploads");

            migrationBuilder.DropTable(
                name: "EquipmentBrands");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_ModelId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Accessory",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "RetirementReason",
                table: "Equipments");

            migrationBuilder.RenameColumn(
                name: "TotalUsageHours",
                table: "Equipments",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "RetiredDate",
                table: "Equipments",
                newName: "LoanDueDate");

            migrationBuilder.RenameColumn(
                name: "PurchasePriceUs",
                table: "Equipments",
                newName: "PurchasePrice");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Equipments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "(sysdatetimeoffset())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Equipments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "(sysdatetimeoffset())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Equipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LoanedTo",
                table: "Equipments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Equipments",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Equipments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpticalDesign",
                table: "Equipments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EQ_CategoryId",
                table: "Equipments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EQ_Category",
                table: "Equipments",
                column: "CategoryId",
                principalTable: "EquipmentCategories",
                principalColumn: "Id");
        }
    }
}
