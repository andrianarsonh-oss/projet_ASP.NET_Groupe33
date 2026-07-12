using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmploiTemps.Models
{
    public class Professeur
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100)]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100)]
        public string? Prenom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Le téléphone est obligatoire")]
        [StringLength(30)]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "La spécialité est obligatoire")]
        [StringLength(100)]
        public string? Specialite { get; set; }

        [StringLength(100)]
        public string? Grade { get; set; }

        [StringLength(100)]
        public string? Diplome { get; set; }

        [StringLength(100)]
        public string? Departement { get; set; }

        [Required(ErrorMessage = "Le matricule est obligatoire")]
        [StringLength(50)]
        public string? MatriculeProfesseur { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateEmbauche { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        [StringLength(50)]
        public string? Statut { get; set; }

        public ICollection<DisponibiliteProfesseur>? Disponibilites { get; set; }

        public ICollection<AffectationProfesseurCours>? Affectations { get; set; }

        public string? PhotoUrl { get; set; }
    }
}