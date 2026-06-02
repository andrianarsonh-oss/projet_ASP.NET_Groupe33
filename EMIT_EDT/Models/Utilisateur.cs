using Microsoft.AspNetCore.Identity;

namespace EMIT_EDT.Models
{
    public class Utilisateur : IdentityUser
    {
        public string Nom { get; set; } = "";
        public string Prenom { get; set; } = "";
        public string Role { get; set; } = "Etudiant";
        public string? Matricule { get; set; }
        public string? PhotoPath { get; set; }
        public bool Actif { get; set; } = true;
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime? DerniereConnexion { get; set; }

        // ✅ AJOUTE CETTE LIGNE
        public Enseignant? Enseignant { get; set; }
    }
}