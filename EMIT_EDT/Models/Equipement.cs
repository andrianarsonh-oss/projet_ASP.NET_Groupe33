using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class Equipement
    {
        [Key]
        public int EquipementId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Icone { get; set; }
    }
}