using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmploiTemps.Models;
using GestionSalleEmploiTemps.Data;

namespace GestionSalleEmploiTemps.Controllers
{
    public class AffectationProfesseurCoursController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Liste officielle des statuts pour l'application
        private static readonly List<string> StatutsValides = new() { "Actif", "Suspendu", "Terminé" };

        public AffectationProfesseurCoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── INDEX (Affichage + Recherche + Filtres) ──────────────────────────
        public IActionResult Index(string? recherche, string? niveau, string? classe, string? statut)
        {
            // Initialisation de la requête de base avec jointure sur le Professeur
            var query = _context.AffectationsProfesseursCours
                .Include(a => a.Professeur)
                .AsQueryable();

            // Filtrage par barre de recherche textuelle
            if (!string.IsNullOrWhiteSpace(recherche))
            {
                var r = recherche.Trim().ToLower();
                query = query.Where(a => a.NomCours!.ToLower().Contains(r) ||
                                         a.CodeCours!.ToLower().Contains(r) ||
                                         a.Professeur!.Nom!.ToLower().Contains(r) ||
                                         a.Professeur!.Prenom!.ToLower().Contains(r));
            }

            // Filtrages stricts depuis les listes déroulantes de l'EMIT
            if (!string.IsNullOrWhiteSpace(niveau)) query = query.Where(a => a.Niveau == niveau.Trim());
            if (!string.IsNullOrWhiteSpace(classe)) query = query.Where(a => a.Classe == classe.Trim());
            if (!string.IsNullOrWhiteSpace(statut)) query = query.Where(a => a.Statut == statut.Trim());

            // Chargement des listes de références pour les filtres de la vue Index
            InitialiserListesReference();

            // Maintien des valeurs sélectionnées dans les inputs de la vue
            ViewBag.Recherche = recherche;
            ViewBag.Niveau = niveau;
            ViewBag.Classe = classe;
            ViewBag.Statut = statut;

            // Retourne la liste triée par nom de cours
            var listeAffectations = query.OrderBy(a => a.NomCours).ToList();
            return View(listeAffectations);
        }

        // ── DETAILS ──────────────────────────────────────────────────────────────
        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();

            var affectation = _context.AffectationsProfesseursCours
                .Include(a => a.Professeur)
                .FirstOrDefault(m => m.Id == id);

            if (affectation == null) return NotFound();

            return View(affectation);
        }

        // ── CREATE GET (Affichage Formulaire) ────────────────────────────────────
        public IActionResult Create()
        {
            InitialiserListesReference();
            return View();
        }

        // ── CREATE POST (Enregistrement) ─────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProfesseurId,NomCours,CodeCours,Niveau,Classe,DateAffectation,Statut")] AffectationProfesseurCours affectation)
        {
            ValiderAffectation(affectation, null);

            if (ModelState.IsValid)
            {
                NormaliserChamps(affectation);
                _context.Add(affectation);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "L'affectation a été enregistrée avec succès.";
                return RedirectToAction(nameof(Index));
            }

            // Réinitialiser les listes déroulantes en cas d'échec de validation
            InitialiserListesReference(affectation.ProfesseurId);
            return View(affectation);
        }

        // ── EDIT GET (Affichage Formulaire avec Données) ─────────────────────────
        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest();

            var affectation = _context.AffectationsProfesseursCours.Find(id);
            if (affectation == null) return NotFound();

            InitialiserListesReference(affectation.ProfesseurId);
            return View(affectation);
        }

        // ── EDIT POST (Mise à jour) ──────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ProfesseurId,NomCours,CodeCours,Niveau,Classe,DateAffectation,Statut")] AffectationProfesseurCours affectation)
        {
            if (id != affectation.Id) return BadRequest();

            ValiderAffectation(affectation, affectation.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    NormaliserChamps(affectation);
                    _context.Update(affectation);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "L'affectation a été mise à jour avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AffectationsProfesseursCours.Any(e => e.Id == affectation.Id)) return NotFound();
                    throw;
                }
            }

            InitialiserListesReference(affectation.ProfesseurId);
            return View(affectation);
        }

        // ── DELETE GET (Confirmation) ────────────────────────────────────────────
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();

            var affectation = _context.AffectationsProfesseursCours
                .Include(a => a.Professeur)
                .FirstOrDefault(m => m.Id == id);

            if (affectation == null) return NotFound();

            return View(affectation);
        }

        // ── DELETE POST (Suppression définitive) ─────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var affectation = _context.AffectationsProfesseursCours.Find(id);
            if (affectation != null)
            {
                _context.AffectationsProfesseursCours.Remove(affectation);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "L'affectation a été supprimée avec succès.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ── ENTRAILLES & UTILITAIRES DU CONTRÔLEUR ───────────────────────────────

        /// <summary>
        /// Vérifie la cohérence des règles métiers de l'établissement
        /// </summary>
        private void ValiderAffectation(AffectationProfesseurCours affectation, int? idExistant)
        {
            // Vérification de l'existence du prof choisi
            if (!_context.Professeurs.Any(p => p.Id == affectation.ProfesseurId))
                ModelState.AddModelError("ProfesseurId", "Veuillez associer un enseignant valide.");

            // Éviter les doublons de cours Actifs pour un même enseignant
            if (!string.IsNullOrWhiteSpace(affectation.NomCours))
            {
                var nom = affectation.NomCours.Trim().ToLower();
                bool doublonExiste = _context.AffectationsProfesseursCours.Any(a =>
                    a.Id != idExistant &&
                    a.ProfesseurId == affectation.ProfesseurId &&
                    a.NomCours!.ToLower() == nom &&
                    a.Statut == "Actif");

                if (doublonExiste)
                    ModelState.AddModelError(string.Empty, "Cet enseignant possède déjà un cours actif portant ce nom.");
            }
        }

        /// <summary>
        /// Nettoie les espaces blancs et applique les valeurs par défaut
        /// </summary>
        private static void NormaliserChamps(AffectationProfesseurCours affectation)
        {
            affectation.NomCours = affectation.NomCours?.Trim();
            affectation.CodeCours = affectation.CodeCours?.Trim();
            affectation.Niveau = affectation.Niveau?.Trim();
            affectation.Classe = affectation.Classe?.Trim();

            if (string.IsNullOrWhiteSpace(affectation.Statut)) affectation.Statut = "Actif";
            if (affectation.DateAffectation == null) affectation.DateAffectation = DateOnly.FromDateTime(DateTime.Today);
        }

        /// <summary>
        /// Initialise les nomenclatures d'études et de parcours de l'EMIT Fianarantsoa
        /// </summary>
        private void InitialiserListesReference(int? professeurId = null)
        {
            // Liste dynamique des enseignants pour le SelectList
            var listeProfs = _context.Professeurs
                .AsNoTracking()
                .OrderBy(p => p.Nom)
                .Select(p => new { p.Id, NomComplet = p.Nom.ToUpper() + " " + p.Prenom })
                .ToList();

            ViewData["ProfesseurId"] = new SelectList(listeProfs, "Id", "NomComplet", professeurId);

            // Niveaux d'études à l'EMIT
            ViewData["NiveauxDisponibles"] = new List<string> {
                "Licence 1 (L1)", "Licence 2 (L2)", "Licence 3 (L3)", "Master 1 (M1)", "Master 2 (M2)"
            };

            // Mentions / Parcours à l'EMIT
            ViewData["ClassesDisponibles"] = new List<string> {
                "Informatique de Gestion (IG)",
                "Administration des Systèmes et Réseaux (ASR)",
                "Génie Logiciel (GL)",
                "Management des Systèmes d'Information (MSI)",
                "Tronc Commun"
            };

            // Statuts applicatifs
            ViewData["Statuts"] = StatutsValides;
        }
    }
}