using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class Salle
    {
        [Key]
        public int SalleId { get; set; }

        [Required, MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        public int Capacite { get; set; }
        public int Etage { get; set; } = 0;

        [MaxLength(20)]
        public string Statut { get; set; } = "Active";

        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        public int BatimentId { get; set; }
        [ForeignKey("BatimentId")]
        public Batiment? Batiment { get; set; }

        public int TypeSalleId { get; set; }
        [ForeignKey("TypeSalleId")]
        public TypeSalle? TypeSalle { get; set; }

        public List<Seance>? Seances { get; set; }

        [MaxLength(100), Column("batiment_salle")]
        public string? BatimentSalle { get; set; }

        [MaxLength(100), Column("position_salle")]
        public string? PositionSalle { get; set; }

        // ✅ Ajout : relation vers les équipements (many-to-many via SalleEquipement)
        public List<SalleEquipement>? SalleEquipements { get; set; }
    }
}