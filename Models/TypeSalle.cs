using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class TypeSalle
    {
        [Key]
        public int TypeSalleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Libelle { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public List<Salle>? Salles { get; set; }
    }
}