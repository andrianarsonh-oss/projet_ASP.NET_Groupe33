using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("DEMANDE")]
    public class Demande
    {
        [Key, Column("id_demande")]
        public int Id { get; set; }

        [Required, Column("dure_debut_demande")]
        public DateTime DureDebut { get; set; }

        [Required, Column("dure_fin_demande")]
        public DateTime DureFin { get; set; }

        [Required, MaxLength(20), Column("code_salle")]
        public string CodeSalle { get; set; } = string.Empty;

        [MaxLength(50), Column("type_demande")]
        public string? TypeDemande { get; set; }

        [MaxLength(200), Column("autre_demande")]
        public string? AutreDemande { get; set; }

        [MaxLength(500), Column("description_demande")]
        public string? Description { get; set; }

        // Clés étrangères
        [Column("id_salle")]
        public int? SalleId { get; set; }
        [ForeignKey("SalleId")]
        public Salle? Salle { get; set; }

        [Column("id_etu")]
        public int EtudiantId { get; set; }
        [ForeignKey("EtudiantId")]
        public Etudiant? Etudiant { get; set; }

        public ICollection<EtreTraiter> Traitements { get; set; } = new List<EtreTraiter>();
    }
}