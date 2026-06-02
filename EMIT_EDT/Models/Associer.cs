using EMIT_EDT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ASSOCIER")]
public class Associer
{
    [Key, Column("id_eds")]
    public int Id { get; set; }

    [Column("id_edt")]
    public int EmploiDuTempsId { get; set; }
    [ForeignKey("EmploiDuTempsId")]
    public EmploiDuTemps? EmploiDuTemps { get; set; }

    [Column("id_salle")]
    public int? SalleId { get; set; }
    [ForeignKey("SalleId")]
    public Salle? Salle { get; set; }

    [Column("id_professeur")]
    public int? ProfesseurId { get; set; }
    [ForeignKey("ProfesseurId")]
    public Professeur? Professeur { get; set; }

    [Column("id_cours")]
    public int? CoursId { get; set; }
    [ForeignKey("CoursId")]
    public Cours? Cours { get; set; }
}