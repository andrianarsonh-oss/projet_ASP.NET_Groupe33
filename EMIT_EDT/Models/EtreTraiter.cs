using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("ETRE_TRAITER")]
    public class EtreTraiter
    {
        [Key, Column("id_suivi")]
        public int Id { get; set; }

        [Column("id_admin")]
        public int AdminId { get; set; }
        [ForeignKey("AdminId")]
        public Admin? Admin { get; set; }

        [Column("id_demande")]
        public int DemandeId { get; set; }
        [ForeignKey("DemandeId")]
        public Demande? Demande { get; set; }
    }
}