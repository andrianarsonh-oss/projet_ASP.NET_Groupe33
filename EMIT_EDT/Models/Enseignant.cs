using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Enseignant
    {
        [Key]
        public int EnseignantId { get; set; }

        [Required]
        public string UtilisateurId { get; set; } = string.Empty;

        [ForeignKey("UtilisateurId")]
        public Utilisateur? Utilisateur { get; set; }

        [Required]
        [MaxLength(20)]
        public string Matricule { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Statut { get; set; } = "Permanent";

        public int ChargeHoraire { get; set; } = 192;

        [MaxLength(200)]
        public string? Specialite { get; set; }

        public List<Seance>? Seances { get; set; }
        public List<MatiereUE>? Matieres { get; set; }

        [MaxLength(50), Column("titre_professeur")]
        public string? Titre { get; set; }

        [Column("dure_travaille_professeur")]
        public int DureTravaille { get; set; } = 192;

        [MaxLength(20), Column("contact_professeur")]
        public string? Contact { get; set; }
    }
}