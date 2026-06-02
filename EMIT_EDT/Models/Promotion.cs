using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(5)]
        public string Niveau { get; set; } = "L1";

        public int Annee { get; set; } = 2025;
        public int Effectif { get; set; }

        public int ParcoursId { get; set; }
        [ForeignKey("ParcoursId")]
        public Parcours? Parcours { get; set; }

        public List<Seance>? Seances { get; set; }
        public List<Groupe>? Groupes { get; set; }
    }
}