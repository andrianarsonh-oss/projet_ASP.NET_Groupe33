using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Parcours
    {
        [Key]
        public int ParcoursId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Grade { get; set; } = "Licence";

        public int MentionId { get; set; }
        [ForeignKey("MentionId")]
        public Mention? Mention { get; set; }

        public List<Promotion>? Promotions { get; set; }
        public List<MatiereUE>? Matieres { get; set; }
    }
}