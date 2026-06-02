using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMIT_EDT.Migrations
{
    /// <inheritdoc />
    public partial class AjoutDemandeEtudiantAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "batiment_salle",
                table: "Salles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "position_salle",
                table: "Salles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_professeur",
                table: "Enseignants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dure_travaille_professeur",
                table: "Enseignants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "titre_professeur",
                table: "Enseignants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ADMIN",
                columns: table => new
                {
                    id_admin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_admin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    pwd_admin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fonction_admin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email_admin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN", x => x.id_admin);
                });

            migrationBuilder.CreateTable(
                name: "ETUDIANT",
                columns: table => new
                {
                    id_etu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    matricule_etu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nom_etu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    prenom_etu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    num_cin_etu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    status_valide = table.Column<bool>(type: "bit", nullable: false),
                    mot_passe_etu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email_etu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETUDIANT", x => x.id_etu);
                });

            migrationBuilder.CreateTable(
                name: "AVOIR",
                columns: table => new
                {
                    id_etu = table.Column<int>(type: "int", nullable: false),
                    id_parcours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AVOIR", x => new { x.id_etu, x.id_parcours });
                    table.ForeignKey(
                        name: "FK_AVOIR_ETUDIANT_id_etu",
                        column: x => x.id_etu,
                        principalTable: "ETUDIANT",
                        principalColumn: "id_etu");
                    table.ForeignKey(
                        name: "FK_AVOIR_Parcours_id_parcours",
                        column: x => x.id_parcours,
                        principalTable: "Parcours",
                        principalColumn: "ParcoursId");
                });

            migrationBuilder.CreateTable(
                name: "DEMANDE",
                columns: table => new
                {
                    id_demande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dure_debut_demande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dure_fin_demande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    code_salle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    type_demande = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    autre_demande = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    description_demande = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    id_salle = table.Column<int>(type: "int", nullable: true),
                    id_etu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEMANDE", x => x.id_demande);
                    table.ForeignKey(
                        name: "FK_DEMANDE_ETUDIANT_id_etu",
                        column: x => x.id_etu,
                        principalTable: "ETUDIANT",
                        principalColumn: "id_etu");
                    table.ForeignKey(
                        name: "FK_DEMANDE_Salles_id_salle",
                        column: x => x.id_salle,
                        principalTable: "Salles",
                        principalColumn: "SalleId");
                });

            migrationBuilder.CreateTable(
                name: "ETRE_TRAITER",
                columns: table => new
                {
                    id_suivi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_admin = table.Column<int>(type: "int", nullable: false),
                    id_demande = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETRE_TRAITER", x => x.id_suivi);
                    table.ForeignKey(
                        name: "FK_ETRE_TRAITER_ADMIN_id_admin",
                        column: x => x.id_admin,
                        principalTable: "ADMIN",
                        principalColumn: "id_admin");
                    table.ForeignKey(
                        name: "FK_ETRE_TRAITER_DEMANDE_id_demande",
                        column: x => x.id_demande,
                        principalTable: "DEMANDE",
                        principalColumn: "id_demande");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AVOIR_id_parcours",
                table: "AVOIR",
                column: "id_parcours");

            migrationBuilder.CreateIndex(
                name: "IX_DEMANDE_id_etu",
                table: "DEMANDE",
                column: "id_etu");

            migrationBuilder.CreateIndex(
                name: "IX_DEMANDE_id_salle",
                table: "DEMANDE",
                column: "id_salle");

            migrationBuilder.CreateIndex(
                name: "IX_ETRE_TRAITER_id_admin",
                table: "ETRE_TRAITER",
                column: "id_admin");

            migrationBuilder.CreateIndex(
                name: "IX_ETRE_TRAITER_id_demande",
                table: "ETRE_TRAITER",
                column: "id_demande");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AVOIR");

            migrationBuilder.DropTable(
                name: "ETRE_TRAITER");

            migrationBuilder.DropTable(
                name: "ADMIN");

            migrationBuilder.DropTable(
                name: "DEMANDE");

            migrationBuilder.DropTable(
                name: "ETUDIANT");

            migrationBuilder.DropColumn(
                name: "batiment_salle",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "position_salle",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "contact_professeur",
                table: "Enseignants");

            migrationBuilder.DropColumn(
                name: "dure_travaille_professeur",
                table: "Enseignants");

            migrationBuilder.DropColumn(
                name: "titre_professeur",
                table: "Enseignants");
        }
    }
}
