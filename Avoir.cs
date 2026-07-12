using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("AVOIR")]
    public class Avoir
    {
        [Key, Column("id_etu")]
        public int EtudiantId { get; set; }
        [ForeignKey("EtudiantId")]
        public Etudiant? Etudiant { get; set; }

        [Column("id_parcours")]
        public int ParcoursId { get; set; }
        [ForeignKey("ParcoursId")]
        public Parcours? Parcours { get; set; }
    }
}