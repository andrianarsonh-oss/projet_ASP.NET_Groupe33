using EMIT_EDT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EMPLOI_DU_TEMPS")]
public class EmploiDuTemps
{
    [Key, Column("id_edt")]
    public int Id { get; set; }

    [Required, MaxLength(20), Column("jours_edt")]
    public string Jours { get; set; } = string.Empty;

    [Required, Column("date_edt")]
    public DateTime Date { get; set; }

    [Required, Column("heure_debut_edt")]
    public TimeSpan HeureDebut { get; set; }

    [Required, Column("heure_fin_edt")]
    public TimeSpan HeureFin { get; set; }

    [Column("id_parcours")]
    public int? ParcoursId { get; set; }
    [ForeignKey("ParcoursId")]
    public Parcours? Parcours { get; set; }

    public ICollection<Associer> Associer { get; set; } = new List<Associer>();
}