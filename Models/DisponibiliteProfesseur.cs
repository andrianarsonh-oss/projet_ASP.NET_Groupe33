using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSalleEmploiTemps.Models
{
    public class DisponibiliteProfesseur
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le professeur est obligatoire")]
        [Display(Name = "Professeur")]
        public int ProfesseurId { get; set; }

        [Required(ErrorMessage = "Le jour est obligatoire")]
        [StringLength(20, ErrorMessage = "Le jour ne doit pas dépasser 20 caractères.")]
        public string Jour { get; set; } = string.Empty; // Initialisé pour éviter les avertissements de nullabilité

        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        [Display(Name = "Heure de début")]
        [DataType(DataType.Time)]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        [Display(Name = "Heure de fin")]
        [DataType(DataType.Time)]
        public TimeSpan HeureFin { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        [StringLength(30, ErrorMessage = "Le statut ne doit pas dépasser 30 caractères.")]
        public string Statut { get; set; } = "Disponible"; // Valeur par défaut logique

        // Relation Entity Framework
        [ForeignKey("ProfesseurId")]
        public virtual Professeur? Professeur { get; set; }
    }
}