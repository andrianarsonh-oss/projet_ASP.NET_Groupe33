using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Groupe
    {
        [Key]
        public int GroupeId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Nom { get; set; } = string.Empty;

        public int Effectif { get; set; }

        public int PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public Promotion? Promotion { get; set; }

        public List<Seance>? Seances { get; set; }
    }
}