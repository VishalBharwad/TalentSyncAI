using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSyncAI.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidatePersonalInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Candidates",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Candidates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Candidates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Candidates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Candidates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "Candidates",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileCompletionPercentage",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Candidates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Candidates",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ProfileCompletionPercentage",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Candidates");
        }
    }
}
