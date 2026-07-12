using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class Semestre
    {
        [Key]
        public int SemestreId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Libelle { get; set; } = string.Empty;

        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        [MaxLength(9)]
        public string AnneeUniversitaire { get; set; } = "2025-2026";

        public bool Actif { get; set; } = true;

        public List<Seance>? Seances { get; set; }
    }
}