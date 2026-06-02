using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class Mention
    {
        [Key]
        public int MentionId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public List<Parcours>? Parcours { get; set; }
    }
}