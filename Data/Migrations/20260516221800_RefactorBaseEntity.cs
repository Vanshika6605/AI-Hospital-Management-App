using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIHospitalManagementSys.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Bills");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PrescriptionItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PrescriptionItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExistingConditions",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DoctorSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DoctorSchedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvailabilityStatus",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ExistingConditions",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "AvailabilityStatus",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Bills");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
