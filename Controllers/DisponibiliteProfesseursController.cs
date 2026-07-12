using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmploiTemps.Data;
using GestionSalleEmploiTemps.Models;

namespace GestionSalleEmploiTemps.Controllers
{
    public class DisponibiliteProfesseursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisponibiliteProfesseursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DisponibiliteProfesseurs
        public async Task<IActionResult> Index(string? search, string? jour, string? statut)
        {
            var query = _context.DisponibilitesProfesseurs
                .Include(d => d.Professeur)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    (d.Professeur != null && d.Professeur.Nom.Contains(search)) ||
                    (d.Professeur != null && d.Professeur.Prenom.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(jour))
            {
                query = query.Where(d => d.Jour == jour);
            }

            if (!string.IsNullOrWhiteSpace(statut))
            {
                query = query.Where(d => d.Statut == statut);
            }

            ViewBag.Search = search;
            ViewBag.Jour = jour;
            ViewBag.Statut = statut;

            ChargerListes();

            return View(await query
                .OrderBy(d => d.Professeur != null ? d.Professeur.Nom : "")
                .ThenBy(d => d.Jour)
                .ThenBy(d => d.HeureDebut)
                .ToListAsync());
        }

        // GET: DisponibiliteProfesseurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disponibilite = await _context.DisponibilitesProfesseurs
                .AsNoTracking() // Performance : pas de suivi requis pour de la lecture
                .Include(d => d.Professeur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (disponibilite == null) return NotFound();

            return View(disponibilite);
        }

        // GET: DisponibiliteProfesseurs/Create
        public IActionResult Create()
        {
            ChargerListes();
            return View();
        }

        // POST: DisponibiliteProfesseurs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProfesseurId,Jour,HeureDebut,HeureFin,Statut,Notes")] DisponibiliteProfesseur disponibilite)
        {
            // Évite les erreurs de validation si l'objet Professeur est requis mais null à la soumission
            disponibilite.Professeur = null;

            // Validation asynchrone des règles métiers
            await ValiderDisponibiliteAsync(disponibilite);

            if (ModelState.IsValid)
            {
                _context.Add(disponibilite);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Disponibilité ajoutée avec succès.";
                return RedirectToAction(nameof(Index));
            }

            ChargerListes(disponibilite.ProfesseurId);
            return View(disponibilite);
        }

        // GET: DisponibiliteProfesseurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var disponibilite = await _context.DisponibilitesProfesseurs.FindAsync(id);

            if (disponibilite == null) return NotFound();

            ChargerListes(disponibilite.ProfesseurId);
            return View(disponibilite);
        }

        // POST: DisponibiliteProfesseurs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfesseurId,Jour,HeureDebut,HeureFin,Statut,Notes")] DisponibiliteProfesseur disponibilite)
        {
            if (id != disponibilite.Id) return NotFound();

            disponibilite.Professeur = null;

            // Validation asynchrone des règles métiers
            await ValiderDisponibiliteAsync(disponibilite);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disponibilite);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Disponibilité modifiée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisponibiliteProfesseurExists(disponibilite.Id))
                        return NotFound();

                    throw;
                }
            }

            ChargerListes(disponibilite.ProfesseurId);
            return View(disponibilite);
        }

        // GET: DisponibiliteProfesseurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disponibilite = await _context.DisponibilitesProfesseurs
                .AsNoTracking()
                .Include(d => d.Professeur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (disponibilite == null) return NotFound();

            return View(disponibilite);
        }

        // POST: DisponibiliteProfesseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disponibilite = await _context.DisponibilitesProfesseurs.FindAsync(id);

            if (disponibilite == null) return NotFound();

            _context.DisponibilitesProfesseurs.Remove(disponibilite);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Disponibilité supprimée avec succès.";
            return RedirectToAction(nameof(Index));
        }

        private bool DisponibiliteProfesseurExists(int id)
        {
            return _context.DisponibilitesProfesseurs.Any(e => e.Id == id);
        }

        private void ChargerListes(int? professeurId = null)
        {
            // Amélioration : On combine le Nom et le Prénom pour l'affichage dans le select
            var listeProfesseurs = _context.Professeurs
                .OrderBy(p => p.Nom)
                .Select(p => new {
                    Id = p.Id,
                    NomComplet = $"{p.Nom.ToUpper()} {p.Prenom}"
                })
                .ToList();

            ViewBag.ProfesseurId = new SelectList(listeProfesseurs, "Id", "NomComplet", professeurId);

            ViewBag.Jours = new List<string>
            {
                "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
            };

            ViewBag.Statuts = new List<string>
            {
                "Disponible", "Occupé", "Indisponible", "En congé"
            };
        }

        private async Task ValiderDisponibiliteAsync(DisponibiliteProfesseur disponibilite)
        {
            // 1. Vérification de la cohérence du créneau horaire
            if (disponibilite.HeureDebut >= disponibilite.HeureFin)
            {
                ModelState.AddModelError("HeureDebut", "L'heure de début doit être strictement inférieure à l'heure de fin.");
            }

            // 2. Vérification asynchrone des chevauchements de planning (Formule mathématique de recouvrement de créneaux)
            bool conflit = await _context.DisponibilitesProfesseurs.AnyAsync(d =>
                d.Id != disponibilite.Id &&
                d.ProfesseurId == disponibilite.ProfesseurId &&
                d.Jour == disponibilite.Jour &&
                d.HeureDebut < disponibilite.HeureFin &&
                disponibilite.HeureDebut < d.HeureFin
            );

            if (conflit)
            {
                ModelState.AddModelError("", "Ce professeur possède déjà un enregistrement qui chevauche ce créneau sur cette même journée.");
            }
        }
    }
}