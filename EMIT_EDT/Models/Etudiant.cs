using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EMIT_EDT.Models
{
    [Table("ETUDIANT")]
    public class Etudiant
    {
        [Key, Column("id_etu")]
        public int Id { get; set; }

        [Required, MaxLength(20), Column("matricule_etu")]
        public string Matricule { get; set; } = string.Empty;

        [Required, MaxLength(50), Column("nom_etu")]
        public string Nom { get; set; } = string.Empty;

        [Required, MaxLength(50), Column("prenom_etu")]
        public string Prenom { get; set; } = string.Empty;

        [MaxLength(20), Column("num_cin_etu")]
        public string? NumCin { get; set; }

        [Column("status_valide")]
        public bool StatutValide { get; set; } = true;

        [Required, MaxLength(100), Column("mot_passe_etu")]
        public string MotPasse { get; set; } = string.Empty;

        [Required, MaxLength(100), Column("email_etu")]
        public string Email { get; set; } = string.Empty;

        // Relations
        public ICollection<Demande> Demandes { get; set; } = new List<Demande>();
        public ICollection<Avoir> Avoirs { get; set; } = new List<Avoir>();
    }
}