using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class Batiment
    {
        [Key]
        public int BatimentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        public int NombreEtages { get; set; } = 1;

        [MaxLength(200)]
        public string? Localisation { get; set; }

        public List<Salle>? Salles { get; set; }
    }
}