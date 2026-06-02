using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    public class MatiereUE
    {
        [Key]
        public int MatiereUEId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Nom { get; set; } = string.Empty;

        public int VolumeCM { get; set; } = 0;
        public int VolumeTD { get; set; } = 0;
        public int VolumeTP { get; set; } = 0;

        public int SemestreId { get; set; }
        [ForeignKey("SemestreId")]
        public Semestre? Semestre { get; set; }

        public List<Seance>? Seances { get; set; }
        public List<Parcours>? Parcours { get; set; }
        public List<Enseignant>? Enseignants { get; set; }
    }
}