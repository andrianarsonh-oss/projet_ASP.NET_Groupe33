using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class CreneauHoraire
    {
        [Key]
        public int CreneauHoraireId { get; set; }

        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        public int Rang { get; set; } = 1;

        public List<Seance>? Seances { get; set; }
    }
}