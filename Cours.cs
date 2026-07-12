using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("COURS")]
public class Cours
{
    [Key, Column("id_cours")]
    public int Id { get; set; }

    [Required, MaxLength(20), Column("code_cours")]
    public string Code { get; set; } = string.Empty;

    [MaxLength(200), Column("description_cours")]
    public string? Description { get; set; }

    public ICollection<Associer> AssocierEmplois { get; set; } = new List<Associer>();
}