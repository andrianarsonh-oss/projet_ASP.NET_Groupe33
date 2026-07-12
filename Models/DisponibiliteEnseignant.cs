using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("DisponibiliteEnseignants")]
    public class DisponibiliteEnseignant
    {
        [Key]  // ← AJOUTE CECI
        public int DisponibiliteEnseignantId { get; set; }

        public int EnseignantId { get; set; }

        [ForeignKey("EnseignantId")]
        public virtual Enseignant? Enseignant { get; set; }

        [Range(1, 6)]
        public int JourSemaine { get; set; }

        public int CreneauHoraireId { get; set; }

        [ForeignKey("CreneauHoraireId")]
        public virtual CreneauHoraire? CreneauHoraire { get; set; }

        public bool Indisponible { get; set; } = false;
    }
}