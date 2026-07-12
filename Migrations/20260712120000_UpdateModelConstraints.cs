using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalleEmploiTemps.Migrations
{
    public partial class UpdateModelConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // DisponibilitesProfesseurs: Jour -> varchar(20), Statut -> varchar(30) with default
            migrationBuilder.AlterColumn<string>(
                name: "Jour",
                table: "DisponibilitesProfesseurs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "DisponibilitesProfesseurs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Disponible",
                oldClrType: typeof(string),
                oldType: "text");

            // AffectationsProfesseursCours: impose longueurs et non-null
            migrationBuilder.AlterColumn<string>(
                name: "NomCours",
                table: "AffectationsProfesseursCours",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CodeCours",
                table: "AffectationsProfesseursCours",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Niveau",
                table: "AffectationsProfesseursCours",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Classe",
                table: "AffectationsProfesseursCours",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "AffectationsProfesseursCours",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Actif",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Disponibilites
            migrationBuilder.AlterColumn<string>(
                name: "Jour",
                table: "DisponibilitesProfesseurs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "DisponibilitesProfesseurs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldDefaultValue: "Disponible");

            // Revert Affectations
            migrationBuilder.AlterColumn<string>(
                name: "NomCours",
                table: "AffectationsProfesseursCours",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CodeCours",
                table: "AffectationsProfesseursCours",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Niveau",
                table: "AffectationsProfesseursCours",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Classe",
                table: "AffectationsProfesseursCours",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "AffectationsProfesseursCours",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Actif");
        }
    }
}
