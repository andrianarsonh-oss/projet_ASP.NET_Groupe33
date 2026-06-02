using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PROFESSEUR")]
public class Professeur
{
    [Key, Column("id_professeur")]
    public int Id { get; set; }

    [Required, MaxLength(50), Column("nom_professeur")]
    public string Nom { get; set; } = string.Empty;

    [Required, MaxLength(50), Column("prenom_professeur")]
    public string Prenom { get; set; } = string.Empty;

    [MaxLength(50), Column("titre_professeur")]
    public string? Titre { get; set; }

    [Column("dure_travaille_professeur")]
    public int DureTravaille { get; set; }

    [MaxLength(20), Column("contact_professeur")]
    public string? Contact { get; set; }

    [Required, MaxLength(100), Column("email_professeur")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100), Column("pwd_professeur")]
    public string Pwd { get; set; } = string.Empty;

    public ICollection<Associer> AssocierEmplois { get; set; } = new List<Associer>();
}