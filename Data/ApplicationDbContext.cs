using GestionSalleEmploiTemps.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmploiTemps.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Professeur> Professeurs { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<DisponibiliteProfesseur> DisponibilitesProfesseurs { get; set; }
        public DbSet<AffectationProfesseurCours> AffectationsProfesseursCours { get; set; }
        

    }

}