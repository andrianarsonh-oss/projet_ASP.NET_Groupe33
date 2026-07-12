using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionSalleEmploiTemps.Migrations
{
    /// <inheritdoc />
    public partial class AjoutAffectationProfesseurCours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AffectationsProfesseursCours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfesseurId = table.Column<int>(type: "integer", nullable: false),
                    NomCours = table.Column<string>(type: "text", nullable: false),
                    CodeCours = table.Column<string>(type: "text", nullable: true),
                    Niveau = table.Column<string>(type: "text", nullable: true),
                    Classe = table.Column<string>(type: "text", nullable: true),
                    DateAffectation = table.Column<DateOnly>(type: "date", nullable: true),
                    Statut = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffectationsProfesseursCours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffectationsProfesseursCours_Professeurs_ProfesseurId",
                        column: x => x.ProfesseurId,
                        principalTable: "Professeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffectationsProfesseursCours_ProfesseurId",
                table: "AffectationsProfesseursCours",
                column: "ProfesseurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffectationsProfesseursCours");
        }
    }
}
