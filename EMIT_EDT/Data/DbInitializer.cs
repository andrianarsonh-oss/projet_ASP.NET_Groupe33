using EMIT_EDT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace EMIT_EDT.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<Utilisateur>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // ===================== RÔLES =====================
            string[] roles = { "SuperAdmin", "RespEDT", "Enseignant", "Etudiant", "Maintenance" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ===================== SUPER ADMIN =====================
            if (await userManager.FindByEmailAsync("admin@emit.mg") == null)
            {
                var admin = new Utilisateur
                {
                    UserName = "admin@emit.mg",
                    Email = "admin@emit.mg",
                    Nom = "Admin",
                    Prenom = "EMIT",
                    Role = "SuperAdmin",
                    Actif = true,
                    DateCreation = DateTime.Now
                };
                await userManager.CreateAsync(admin, "Emit@2026!");
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }

            // ===================== 3 MENTIONS =====================
            if (!context.Mentions.Any())
            {
                context.Mentions.AddRange(
                    new Mention { Code = "MGMT", Nom = "Management" },
                    new Mention { Code = "INFO", Nom = "Informatique" },
                    new Mention { Code = "RPM", Nom = "Relations Publiques et Multimédia" }
                );
                await context.SaveChangesAsync();
            }

            // ===================== 10 PARCOURS =====================
            if (!context.Parcours.Any())
            {
                context.Parcours.AddRange(
                    new Parcours { Code = "AES", Nom = "Administration Économique et Sociale", Grade = "Licence", MentionId = 1 },
                    new Parcours { Code = "MD", Nom = "Management Décisionnel", Grade = "Master", MentionId = 1 },
                    new Parcours { Code = "DA2I", Nom = "Développement Application Internet/Intranet", Grade = "Licence", MentionId = 2 },
                    new Parcours { Code = "CIGSI", Nom = "Conception Intégration Systèmes Information", Grade = "Licence", MentionId = 2 },
                    new Parcours { Code = "SIGD", Nom = "Système Information Géomatique Décision", Grade = "Master", MentionId = 2 },
                    new Parcours { Code = "M2I", Nom = "Modélisation Ingénierie Informatique", Grade = "Master", MentionId = 2 },
                    new Parcours { Code = "CM", Nom = "Communication Multimédia", Grade = "Licence", MentionId = 3 },
                    new Parcours { Code = "RPCO", Nom = "Relations Publiques Communication Organisationnelle", Grade = "Licence", MentionId = 3 },
                    new Parcours { Code = "CMN", Nom = "Communications Médias Numériques", Grade = "Master", MentionId = 3 },
                    new Parcours { Code = "RPC", Nom = "Relations Publiques Communications", Grade = "Master", MentionId = 3 }
                );
                await context.SaveChangesAsync();
            }

            // ===================== SEMESTRES =====================
            if (!context.Semestres.Any())
            {
                context.Semestres.AddRange(
                    new Semestre { Libelle = "Semestre 1", DateDebut = new DateTime(2025, 9, 1), DateFin = new DateTime(2025, 12, 20), AnneeUniversitaire = "2025-2026", Actif = true },
                    new Semestre { Libelle = "Semestre 2", DateDebut = new DateTime(2026, 1, 5), DateFin = new DateTime(2026, 4, 30), AnneeUniversitaire = "2025-2026", Actif = true }
                );
                await context.SaveChangesAsync();
            }

            // ===================== MATIÈRES / UE =====================
            if (!context.MatiereUEs.Any())
            {
                context.MatiereUEs.AddRange(
                    new MatiereUE { Code = "BDD101", Nom = "Bases de données", VolumeCM = 20, VolumeTD = 10, VolumeTP = 10, SemestreId = 1 },
                    new MatiereUE { Code = "ALGO101", Nom = "Algorithmique", VolumeCM = 15, VolumeTD = 15, VolumeTP = 15, SemestreId = 1 },
                    new MatiereUE { Code = "WEB201", Nom = "Développement Web", VolumeCM = 10, VolumeTD = 10, VolumeTP = 20, SemestreId = 2 }
                );
                await context.SaveChangesAsync();
            }

            // ===================== ENSEIGNANTS =====================
            if (!context.Enseignants.Any())
            {
                var prof1 = new Utilisateur
                {
                    UserName = "rakoto@emit.mg",
                    Email = "rakoto@emit.mg",
                    Nom = "Rakoto",
                    Prenom = "Jean",
                    Role = "Enseignant",
                    Actif = true,
                    DateCreation = DateTime.Now
                };
                await userManager.CreateAsync(prof1, "Emit@2026!");
                await userManager.AddToRoleAsync(prof1, "Enseignant");

                context.Enseignants.Add(new Enseignant
                {
                    UtilisateurId = prof1.Id,
                    Matricule = "EN001",
                    Statut = "Permanent",
                    ChargeHoraire = 192
                });

                var prof2 = new Utilisateur
                {
                    UserName = "rabe@emit.mg",
                    Email = "rabe@emit.mg",
                    Nom = "Rabe",
                    Prenom = "Marie",
                    Role = "Enseignant",
                    Actif = true,
                    DateCreation = DateTime.Now
                };
                await userManager.CreateAsync(prof2, "Emit@2026!");
                await userManager.AddToRoleAsync(prof2, "Enseignant");

                context.Enseignants.Add(new Enseignant
                {
                    UtilisateurId = prof2.Id,
                    Matricule = "EN002",
                    Statut = "Vacataire",
                    ChargeHoraire = 96
                });

                await context.SaveChangesAsync();
            }

            // ===================== PROMOTIONS =====================
            if (!context.Promotions.Any())
            {
                context.Promotions.AddRange(
                    new Promotion { Code = "L1-DA2I", Niveau = "L1", Annee = 2025, Effectif = 80, ParcoursId = 3 },
                    new Promotion { Code = "L2-DA2I", Niveau = "L2", Annee = 2025, Effectif = 65, ParcoursId = 3 },
                    new Promotion { Code = "L1-AES", Niveau = "L1", Annee = 2025, Effectif = 100, ParcoursId = 1 }
                );
                await context.SaveChangesAsync();
            }

            // ===================== BÂTIMENTS =====================
            if (!context.Batiments.Any())
            {
                context.Batiments.AddRange(
                    new Batiment { Nom = "Bâtiment A", NombreEtages = 2, Localisation = "Campus principal" },
                    new Batiment { Nom = "Bâtiment B", NombreEtages = 3, Localisation = "Annexe" }
                );
                await context.SaveChangesAsync();
            }

            // ===================== TYPES DE SALLE (si pas déjà faits) =====================
            if (!context.TypeSalles.Any())
            {
                context.TypeSalles.AddRange(
                    new TypeSalle { Libelle = "Amphithéâtre" },
                    new TypeSalle { Libelle = "Salle standard" },
                    new TypeSalle { Libelle = "Laboratoire informatique" },
                    new TypeSalle { Libelle = "Salle de TD" },
                    new TypeSalle { Libelle = "Salle de conférence" }
                );
                await context.SaveChangesAsync();
            }

            // ===================== CRÉNEAUX HORAIRES =====================
            if (!context.CreneauHoraires.Any())
            {
                context.CreneauHoraires.AddRange(
                    new CreneauHoraire { HeureDebut = new TimeSpan(8, 0, 0), HeureFin = new TimeSpan(10, 0, 0), Rang = 1 },
                    new CreneauHoraire { HeureDebut = new TimeSpan(10, 15, 0), HeureFin = new TimeSpan(12, 15, 0), Rang = 2 },
                    new CreneauHoraire { HeureDebut = new TimeSpan(14, 0, 0), HeureFin = new TimeSpan(16, 0, 0), Rang = 3 },
                    new CreneauHoraire { HeureDebut = new TimeSpan(16, 15, 0), HeureFin = new TimeSpan(18, 15, 0), Rang = 4 }
                );
                await context.SaveChangesAsync();
            }

            // ===================== ÉQUIPEMENTS =====================
            if (!context.Equipements.Any())
            {
                context.Equipements.AddRange(
                    new Equipement { Nom = "Vidéoprojecteur", Icone = "fa-video" },
                    new Equipement { Nom = "Ordinateurs", Icone = "fa-desktop" },
                    new Equipement { Nom = "Système audio", Icone = "fa-volume-up" },
                    new Equipement { Nom = "Climatisation", Icone = "fa-snowflake" },
                    new Equipement { Nom = "WiFi/Internet", Icone = "fa-wifi" },
                    new Equipement { Nom = "Tableau numérique", Icone = "fa-chalkboard-teacher" }
                );
                await context.SaveChangesAsync();
            }

            // ===================== SALLES =====================
            if (!context.Salles.Any())
            {
                // Récupérer les bâtiments et types de salle (FirstOrDefault avec vérification)
                var batA = await context.Batiments.FirstOrDefaultAsync(b => b.Nom == "Bâtiment A");
                var batB = await context.Batiments.FirstOrDefaultAsync(b => b.Nom == "Bâtiment B");
                var typeStandard = await context.TypeSalles.FirstOrDefaultAsync(t => t.Libelle == "Salle standard");
                var typeLabo = await context.TypeSalles.FirstOrDefaultAsync(t => t.Libelle == "Laboratoire informatique");

                if (batA == null || batB == null || typeStandard == null || typeLabo == null)
                {
                    // Si l'un d'eux est null, on ne peut pas créer les salles correctement, mais on continue sans plante
                    return;
                }

                context.Salles.AddRange(
                    new Salle { Code = "A-101", Nom = "Salle A-101", Capacite = 80, Etage = 1, BatimentId = batA.BatimentId, TypeSalleId = typeStandard.TypeSalleId, Statut = "Active" },
                    new Salle { Code = "A-102", Nom = "Labo A-102", Capacite = 40, Etage = 1, BatimentId = batA.BatimentId, TypeSalleId = typeLabo.TypeSalleId, Statut = "Active" },
                    new Salle { Code = "B-201", Nom = "Amphi B-201", Capacite = 200, Etage = 2, BatimentId = batB.BatimentId, TypeSalleId = typeStandard.TypeSalleId, Statut = "Active" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}