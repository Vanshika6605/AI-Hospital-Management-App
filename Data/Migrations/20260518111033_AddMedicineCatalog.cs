using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIHospitalManagementSys.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicineCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "PrescriptionItems");

            migrationBuilder.RenameColumn(
                name: "DurationDays",
                table: "PrescriptionItems",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "MedicineId",
                table: "PrescriptionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedicineCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineCatalogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_MedicineId",
                table: "PrescriptionItems",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_MedicineCatalogs_MedicineId",
                table: "PrescriptionItems",
                column: "MedicineId",
                principalTable: "MedicineCatalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionItems_MedicineCatalogs_MedicineId",
                table: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "MedicineCatalogs");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionItems_MedicineId",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "PrescriptionItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PrescriptionItems",
                newName: "DurationDays");

            migrationBuilder.AddColumn<string>(
                name: "MedicineName",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstructions",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
