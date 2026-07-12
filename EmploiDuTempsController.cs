using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmploiTemps.Data;
using GestionEmploiTemps.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionEmploiTemps.Controllers
{
    public class EmploiDuTempsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmploiDuTempsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE
        public async Task<IActionResult> Index()
        {
            var emplois = await _context.EmploiDuTemps
                .Include(e => e.Matiere)
                .Include(e => e.Professeur)
                .Include(e => e.Salle)
                .ToListAsync();

            return View(emplois);
        }

        // FORMULAIRE CREATE
        public IActionResult Create()
        {
            ViewBag.Matieres = new SelectList(_context.Matieres, "Id", "NomMatiere");

            ViewBag.Professeurs = new SelectList(_context.Professeurs, "Id", "Nom");

            ViewBag.Salles = new SelectList(_context.Salles, "Id", "NomSalle");

            ViewBag.Jours = new SelectList(new[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" });

            return View();
        }

        // ENREGISTREMENT CREATE
        [HttpPost]
        public async Task<IActionResult> Create(EmploiDuTemps emploi)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Matieres = new SelectList(_context.Matieres, "Id", "NomMatiere");
                ViewBag.Professeurs = new SelectList(_context.Professeurs, "Id", "Nom");
                ViewBag.Salles = new SelectList(_context.Salles, "Id", "NomSalle");
                ViewBag.Jours = new SelectList(new[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" });
                return View(emploi);
            }

            bool conflit = _context.EmploiDuTemps.Any(e =>
                e.Jour == emploi.Jour &&

                (
                    e.ProfesseurId == emploi.ProfesseurId ||
                    e.SalleId == emploi.SalleId
                ) &&

                (
                    emploi.HeureDebut < e.HeureFin &&
                    emploi.HeureFin > e.HeureDebut
                )
            );

            // Vérification conflits pour le groupe d'étudiants si renseigné
            if (!string.IsNullOrWhiteSpace(emploi.Groupe))
            {
                bool conflitEtudiants = _context.EmploiDuTemps.Any(e =>
                    e.Jour == emploi.Jour &&
                    e.Groupe == emploi.Groupe &&
                    (emploi.HeureDebut < e.HeureFin && emploi.HeureFin > e.HeureDebut)
                );

                if (conflitEtudiants)
                {
                    conflit = true;
                }
            }

            if (conflit)
            {
                ModelState.AddModelError("", "Conflit détecté : professeur ou salle déjà occupé.");

                ViewBag.Matieres = new SelectList(_context.Matieres, "Id", "NomMatiere");
                ViewBag.Professeurs = new SelectList(_context.Professeurs, "Id", "Nom");
                ViewBag.Salles = new SelectList(_context.Salles, "Id", "NomSalle");

                return View(emploi);
            }

            _context.EmploiDuTemps.Add(emploi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Vue calendrier
        public IActionResult Calendar()
        {
            return View();
        }

        // Fournit les événements pour FullCalendar
        [HttpGet]
        public async Task<JsonResult> Events(int? semaine)
        {
            var emplois = await _context.EmploiDuTemps
                .Include(e => e.Matiere)
                .Include(e => e.Professeur)
                .Include(e => e.Salle)
                .ToListAsync();

            int diff = (7 + ((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday)) % 7;
            DateTime monday = DateTime.Today.AddDays(-diff);

            var dayMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "lundi", 0 }, { "mardi", 1 }, { "mercredi", 2 }, { "jeudi", 3 }, { "vendredi", 4 }, { "samedi", 5 }, { "dimanche", 6 }
            };

            var list = emplois.Select(e =>
            {
                var jourKey = e.Jour ?? string.Empty;
                int dayOffset = dayMap.ContainsKey(jourKey) ? dayMap[jourKey] : 0;
                DateTime date = monday.AddDays(dayOffset + (e.Semaine > 0 ? (e.Semaine - 1) * 7 : 0));
                var start = date.Add(e.HeureDebut);
                var end = date.Add(e.HeureFin);

                return new
                {
                    id = e.Id,
                    title = (e.Matiere?.NomMatiere ?? "") + " - " + (e.Professeur?.Nom ?? ""),
                    start = start.ToString("s"),
                    end = end.ToString("s"),
                    extendedProps = new { salle = e.Salle?.NomSalle, groupe = e.Groupe }
                };
            }).ToList();

            return Json(list);
        }
    }
}