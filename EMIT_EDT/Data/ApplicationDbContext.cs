using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Models;

namespace EMIT_EDT.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Mention> Mentions => Set<Mention>();
        public DbSet<Parcours> Parcours => Set<Parcours>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<Semestre> Semestres => Set<Semestre>();
        public DbSet<Batiment> Batiments => Set<Batiment>();
        public DbSet<TypeSalle> TypeSalles => Set<TypeSalle>();
        public DbSet<Salle> Salles => Set<Salle>();
        public DbSet<Enseignant> Enseignants => Set<Enseignant>();
        public DbSet<Groupe> Groupes => Set<Groupe>();
        public DbSet<MatiereUE> MatiereUEs => Set<MatiereUE>();
        public DbSet<CreneauHoraire> CreneauHoraires => Set<CreneauHoraire>();
        public DbSet<Seance> Seances => Set<Seance>();
        public DbSet<Equipement> Equipements => Set<Equipement>();
        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<SalleEquipement> SalleEquipements => Set<SalleEquipement>();

        public DbSet<Etudiant> Etudiants => Set<Etudiant>();
        public DbSet<Demande> Demandes => Set<Demande>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<EtreTraiter> EtreTraiters => Set<EtreTraiter>();
        public DbSet<Avoir> Avoirs => Set<Avoir>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clé composite SalleEquipement
            modelBuilder.Entity<SalleEquipement>()
                .HasKey(se => new { se.SalleId, se.EquipementId });

            // MatiereUE <-> Parcours (many-to-many)
            modelBuilder.Entity<MatiereUE>()
                .HasMany(m => m.Parcours)
                .WithMany(p => p.Matieres)
                .UsingEntity(j => j.ToTable("MatiereParcours"));

            // MatiereUE <-> Enseignant (many-to-many)
            modelBuilder.Entity<MatiereUE>()
                .HasMany(m => m.Enseignants)
                .WithMany(e => e.Matieres)
                .UsingEntity(j => j.ToTable("MatiereEnseignants"));

            // Enseignant <-> Utilisateur (one-to-one)
            modelBuilder.Entity<Enseignant>()
                .HasOne(e => e.Utilisateur)
                .WithOne(u => u.Enseignant)
                .HasForeignKey<Enseignant>(e => e.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // ⭐ CORRECTION : Éviter les cycles de suppression en cascade
            modelBuilder.Entity<Seance>()
                .HasOne(s => s.Semestre)
                .WithMany(s => s.Seances)
                .HasForeignKey(s => s.SemestreId)
                .OnDelete(DeleteBehavior.NoAction);

            // Si nécessaire, ajoute aussi pour d'autres relations
            modelBuilder.Entity<Seance>()
                .HasOne(s => s.Groupe)
                .WithMany(g => g.Seances)
                .HasForeignKey(s => s.GroupeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Etudiant - Demande (1-N)
            modelBuilder.Entity<Demande>()
                .HasOne(d => d.Etudiant)
                .WithMany(e => e.Demandes)
                .HasForeignKey(d => d.EtudiantId)
                .OnDelete(DeleteBehavior.NoAction);

            // Demande - Salle (0-1)
            modelBuilder.Entity<Demande>()
                .HasOne(d => d.Salle)
                .WithMany()
                .HasForeignKey(d => d.SalleId)
                .OnDelete(DeleteBehavior.NoAction);

            // EtreTraiter - Admin
            modelBuilder.Entity<EtreTraiter>()
                .HasOne(et => et.Admin)
                .WithMany(a => a.Traitements)
                .HasForeignKey(et => et.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            // EtreTraiter - Demande
            modelBuilder.Entity<EtreTraiter>()
                .HasOne(et => et.Demande)
                .WithMany(d => d.Traitements)
                .HasForeignKey(et => et.DemandeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Avoir - Etudiant / Parcours
            modelBuilder.Entity<Avoir>()
                .HasKey(a => new { a.EtudiantId, a.ParcoursId });

            modelBuilder.Entity<Avoir>()
                .HasOne(a => a.Etudiant)
                .WithMany(e => e.Avoirs)
                .HasForeignKey(a => a.EtudiantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Avoir>()
                .HasOne(a => a.Parcours)
                .WithMany()
                .HasForeignKey(a => a.ParcoursId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}