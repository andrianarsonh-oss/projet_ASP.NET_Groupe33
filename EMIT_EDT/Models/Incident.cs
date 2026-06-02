using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("Incidents")]
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }

        public int SalleId { get; set; }
        [ForeignKey("SalleId")]
        public virtual Salle? Salle { get; set; }

        [Required]
        [StringLength(50)]
        public string TypePanne { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 3)]
        public int NiveauUrgence { get; set; } = 1;

        public string? PhotoPath { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "Signale"; // Signale, EnCours, Resolu

        public string? SignaleParId { get; set; }
        public string? TechnicienId { get; set; }

        public DateTime DateSignalement { get; set; } = DateTime.Now;
        public DateTime? DateResolution { get; set; }
        public string? CommentaireResolution { get; set; }
    }
}