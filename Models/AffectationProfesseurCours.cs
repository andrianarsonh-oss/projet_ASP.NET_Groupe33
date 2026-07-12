using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSalleEmploiTemps.Models
{
    public class AffectationProfesseurCours
    {
        [Key]
        public int Id { get; set; }

        // Clé étrangère vers l'enseignant
        [Required(ErrorMessage = "L'enseignant est obligatoire.")]
        [Display(Name = "Enseignant")]
        public int ProfesseurId { get; set; }

        [ForeignKey("ProfesseurId")]
        public virtual Professeur? Professeur { get; set; }

        // Informations sur le cours
        [Required(ErrorMessage = "Le nom du cours est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Intitulé du Cours")]
        public string? NomCours { get; set; }

        [Required(ErrorMessage = "Le code du cours est obligatoire.")]
        [StringLength(20)]
        [Display(Name = "Code Matière")]
        public string? CodeCours { get; set; }

        // Niveaux et Classes (Parcours EMIT)
        [Required(ErrorMessage = "Le niveau est obligatoire.")]
        [StringLength(50)]
        public string? Niveau { get; set; }

        [Required(ErrorMessage = "La classe/parcours est obligatoire.")]
        [StringLength(100)]
        public string? Classe { get; set; }

        // Date d'affectation
        [Display(Name = "Date d'affectation")]
        [DataType(DataType.Date)]
        public DateOnly? DateAffectation { get; set; }

        // Statut (ex: Actif, Suspendu, Terminé)
        [Required]
        [StringLength(20)]
        public string? Statut { get; set; } = "Actif";
    }
}