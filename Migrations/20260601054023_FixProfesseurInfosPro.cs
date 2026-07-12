using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalleEmploiTemps.Migrations
{
    /// <inheritdoc />
    public partial class FixProfesseurInfosPro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateEmbauche",
                table: "Professeurs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departement",
                table: "Professeurs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diplome",
                table: "Professeurs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Professeurs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatriculeProfesseur",
                table: "Professeurs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Professeurs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEmbauche",
                table: "Professeurs");

            migrationBuilder.DropColumn(
                name: "Departement",
                table: "Professeurs");

            migrationBuilder.DropColumn(
                name: "Diplome",
                table: "Professeurs");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Professeurs");

            migrationBuilder.DropColumn(
                name: "MatriculeProfesseur",
                table: "Professeurs");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Professeurs");
        }
    }
}
