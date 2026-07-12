using Microsoft.AspNetCore.Identity;
using EMIT_EDT.Models;

namespace EMIT_EDT.Data
{
    /// <summary>
    /// Classe pour initialiser les données de test dans la base de données
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Ajoute les utilisateurs de test à la base de données
        /// </summary>
        public static async Task SeedTestUsersAsync(
            UserManager<Utilisateur> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 🔐 Créer les rôles s'ils n'existent pas
            string[] roleNames = { "Admin", "Enseignant", "Etudiant" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 👤 Utilisateur Test - Admin
            var adminUser = new Utilisateur
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true,
                Nom = "Test",
                Prenom = "Admin",
                Role = "Admin",
                Actif = true,
                DateCreation = DateTime.Now
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 👨‍🏫 Utilisateur Test - Enseignant
            var enseignantUser = new Utilisateur
            {
                UserName = "enseignant@test.com",
                Email = "enseignant@test.com",
                EmailConfirmed = true,
                Nom = "Dupont",
                Prenom = "Jean",
                Role = "Enseignant",
                Matricule = "ENS001",
                Actif = true,
                DateCreation = DateTime.Now
            };

            if (await userManager.FindByEmailAsync(enseignantUser.Email) == null)
            {
                var result = await userManager.CreateAsync(enseignantUser, "Enseignant@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(enseignantUser, "Enseignant");
                }
            }

            // 🎓 Utilisateur Test - Étudiant
            var etudiantUser = new Utilisateur
            {
                UserName = "etudiant@test.com",
                Email = "etudiant@test.com",
                EmailConfirmed = true,
                Nom = "Martin",
                Prenom = "Pierre",
                Role = "Etudiant",
                Matricule = "STU001",
                Actif = true,
                DateCreation = DateTime.Now
            };

            if (await userManager.FindByEmailAsync(etudiantUser.Email) == null)
            {
                var result = await userManager.CreateAsync(etudiantUser, "Etudiant@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(etudiantUser, "Etudiant");
                }
            }
        }
    }
}
