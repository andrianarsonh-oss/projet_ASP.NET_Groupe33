using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.EntityFrameworkCore;

namespace EMIT_EDT.Services
{
    /// <summary>
    /// Service de détection et gestion des conflits d'emploi du temps
    /// </summary>
    public class ConflitService
    {
        private readonly ApplicationDbContext _context;

        public ConflitService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Vérifie tous les conflits possibles avant création d'une séance
        /// </summary>
        public async Task<ConflitResult> VerifierConflits(Seance nouvelleSeance)
        {
            var result = new ConflitResult { AConflit = false, Messages = new List<string>() };

            // 1. Conflit de salle
            var conflitSalle = await VerifierConflitSalle(nouvelleSeance);
            if (conflitSalle)
            {
                result.AConflit = true;
                result.EstBloquant = true;
                result.Messages.Add($"La salle est déjà occupée ce créneau.");
            }

            // 2. Conflit enseignant
            var conflitEnseignant = await VerifierConflitEnseignant(nouvelleSeance);
            if (conflitEnseignant)
            {
                result.AConflit = true;
                result.EstBloquant = true;
                result.Messages.Add($"L'enseignant a déjà un cours ce créneau.");
            }

            // 3. Conflit classe/groupe
            var conflitClasse = await VerifierConflitClasse(nouvelleSeance);
            if (conflitClasse)
            {
                result.AConflit = true;
                result.EstBloquant = true;
                result.Messages.Add($"La classe/groupe a déjà un cours ce créneau.");
            }

            // 4. Capacité dépassée
            var capaciteIssue = await VerifierCapacite(nouvelleSeance);
            if (capaciteIssue)
            {
                result.AConflit = true;
                result.EstBloquant = false; // Alerte mais pas bloquant forcément
                result.Messages.Add($"La capacité de la salle est inférieure à l'effectif.");
            }

            // 5. Type de salle inadapté
            var typeInadapte = await VerifierTypeSalle(nouvelleSeance);
            if (typeInadapte)
            {
                result.AConflit = true;
                result.EstBloquant = false;
                result.Messages.Add($"Le type de salle n'est pas adapté au type de séance.");
            }

            // 6. Salle en maintenance
            var salleMaintenance = await VerifierSalleMaintenance(nouvelleSeance);
            if (salleMaintenance)
            {
                result.AConflit = true;
                result.EstBloquant = true;
                result.Messages.Add($"La salle est en maintenance.");
            }

            return result;
        }

        private async Task<bool> VerifierConflitSalle(Seance seance)
        {
            return await _context.Seances
                .Where(s => s.SalleId == seance.SalleId
                    && s.JourSemaine == seance.JourSemaine
                    && s.CreneauHoraireId == seance.CreneauHoraireId
                    && s.Statut == "Actif"
                    && s.SeanceId != seance.SeanceId
                    && s.DateFinValidite >= DateTime.Now)
                .AnyAsync();
        }

        private async Task<bool> VerifierConflitEnseignant(Seance seance)
        {
            return await _context.Seances
                .Where(s => s.EnseignantId == seance.EnseignantId
                    && s.JourSemaine == seance.JourSemaine
                    && s.CreneauHoraireId == seance.CreneauHoraireId
                    && s.Statut == "Actif"
                    && s.SeanceId != seance.SeanceId
                    && s.DateFinValidite >= DateTime.Now)
                .AnyAsync();
        }

        private async Task<bool> VerifierConflitClasse(Seance seance)
        {
            return await _context.Seances
                .Where(s => s.PromotionId == seance.PromotionId
                    && s.JourSemaine == seance.JourSemaine
                    && s.CreneauHoraireId == seance.CreneauHoraireId
                    && s.Statut == "Actif"
                    && s.SeanceId != seance.SeanceId
                    && s.DateFinValidite >= DateTime.Now
                    && (s.GroupeId == null || seance.GroupeId == null || s.GroupeId == seance.GroupeId))
                .AnyAsync();
        }

        private async Task<bool> VerifierCapacite(Seance seance)
        {
            var salle = await _context.Salles.FindAsync(seance.SalleId);
            var promotion = await _context.Promotions
                .Include(p => p.Groupes)
                .FirstOrDefaultAsync(p => p.PromotionId == seance.PromotionId);

            if (salle == null || promotion == null) return false;

            int effectif = seance.GroupeId != null
                ? promotion.Groupes?.FirstOrDefault(g => g.GroupeId == seance.GroupeId)?.Effectif ?? promotion.Effectif
                : promotion.Effectif;

            return effectif > salle.Capacite;
        }

        private async Task<bool> VerifierTypeSalle(Seance seance)
        {
            var salle = await _context.Salles
                .Include(s => s.TypeSalle)
                .FirstOrDefaultAsync(s => s.SalleId == seance.SalleId);

            if (salle == null) return false;

            // Si TP, obligatoire Labo info
            if (seance.Type == "TP" && salle.TypeSalle?.Libelle != "Laboratoire informatique")
                return true;

            return false;
        }

        private async Task<bool> VerifierSalleMaintenance(Seance seance)
        {
            var salle = await _context.Salles.FindAsync(seance.SalleId);
            return salle?.Statut == "EnMaintenance";
        }

        /// <summary>
        /// Trouve les salles disponibles pour un créneau donné
        /// </summary>
        public async Task<List<Salle>> GetSallesDisponibles(int jour, int creneauId, int effectif, string typeSeance)
        {
            var sallesOccupees = await _context.Seances
                .Where(s => s.JourSemaine == jour
                    && s.CreneauHoraireId == creneauId
                    && s.Statut == "Actif")
                .Select(s => s.SalleId)
                .ToListAsync();

            var query = _context.Salles
                .Include(s => s.TypeSalle)
                .Include(s => s.Batiment)
                .Where(s => !sallesOccupees.Contains(s.SalleId)
                    && s.Statut == "Active"
                    && s.Capacite >= effectif);

            // Filtrer par type pour les TP
            if (typeSeance == "TP")
            {
                query = query.Where(s => s.TypeSalle!.Libelle == "Laboratoire informatique");
            }
            else if (typeSeance == "CM" && effectif > 100)
            {
                query = query.Where(s => s.TypeSalle!.Libelle == "Amphithéâtre" || s.Capacite >= effectif);
            }

            return await query
                .OrderBy(s => s.Capacite)
                .ToListAsync();
        }
    }

    public class ConflitResult
    {
        public bool AConflit { get; set; }
        public bool EstBloquant { get; set; }
        public List<string> Messages { get; set; } = new();
    }
}