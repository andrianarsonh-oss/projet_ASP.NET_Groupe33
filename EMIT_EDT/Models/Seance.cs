using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Seance
    {
        [Key]
        public int SeanceId { get; set; }

        public int JourSemaine { get; set; }

        [MaxLength(3)]
        public string Type { get; set; } = "CM";

        public DateTime DateDebutValidite { get; set; }
        public DateTime DateFinValidite { get; set; }

        [MaxLength(7)]
        public string? Couleur { get; set; } = "#1a5276";

        [MaxLength(20)]
        public string Statut { get; set; } = "Actif";

        public int MatiereUEId { get; set; }
        [ForeignKey("MatiereUEId")]
        public MatiereUE? MatiereUE { get; set; }

        public int EnseignantId { get; set; }
        [ForeignKey("EnseignantId")]
        public Enseignant? Enseignant { get; set; }

        public int SalleId { get; set; }
        [ForeignKey("SalleId")]
        public Salle? Salle { get; set; }

        public int CreneauHoraireId { get; set; }
        [ForeignKey("CreneauHoraireId")]
        public CreneauHoraire? CreneauHoraire { get; set; }

        public int PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public Promotion? Promotion { get; set; }

        public int? GroupeId { get; set; }
        [ForeignKey("GroupeId")]
        public Groupe? Groupe { get; set; }

        public int SemestreId { get; set; }
        [ForeignKey("SemestreId")]
        public Semestre? Semestre { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime? DateModification { get; set; }

        [MaxLength(450)]
        public string? CreeParId { get; set; }

        [MaxLength(500)]
        public string? MotifAnnulation { get; set; }
    }
}