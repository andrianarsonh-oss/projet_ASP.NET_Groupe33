using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMIT_EDT.Models
{
    [Table("ADMIN")]
    public class Admin
    {
        [Key, Column("id_admin")]
        public int Id { get; set; }

        [Required, MaxLength(50), Column("nom_admin")]
        public string Nom { get; set; } = string.Empty;

        [Required, MaxLength(100), Column("pwd_admin")]
        public string Pwd { get; set; } = string.Empty;

        [MaxLength(50), Column("fonction_admin")]
        public string? Fonction { get; set; }

        [Required, MaxLength(100), Column("email_admin")]
        public string Email { get; set; } = string.Empty;

        public ICollection<EtreTraiter> Traitements { get; set; } = new List<EtreTraiter>();
    }
}