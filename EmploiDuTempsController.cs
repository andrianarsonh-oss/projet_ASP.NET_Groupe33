using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;  // ← ZAVA-DEHIBE
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using EMIT_EDT.Services;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class EmploiDuTempsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ConflitService _conflitService;

        public EmploiDuTempsController(ApplicationDbContext context, ConflitService conflitService)
        {
            _context = context;
            _conflitService = conflitService;
        }

        public async Task<IActionResult> Index(int? parcoursId)
        {
            ViewBag.Parcours = await _context.Parcours.Include(p => p.Mention).ToListAsync();
            ViewBag.Creneaux = await _context.CreneauHoraires.OrderBy(c => c.HeureDebut).ToListAsync();

            var seances = _context.Seances
                .Include(s => s.MatiereUE)
                .Include(s => s.Enseignant).ThenInclude(e => e!.Utilisateur)
                .Include(s => s.Salle).ThenInclude(s => s!.Batiment)
                .Include(s => s.CreneauHoraire)
                .Include(s => s.Promotion).ThenInclude(p => p!.Parcours)
                .Include(s => s.Groupe)
                .Where(s => s.Statut == "Actif");

            if (parcoursId.HasValue)
                seances = seances.Where(s => s.Promotion!.ParcoursId == parcoursId.Value);

            return View(await seances.OrderBy(s => s.JourSemaine).ThenBy(s => s.CreneauHoraire!.HeureDebut).ToListAsync());
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create()
        {
            await RemplirListes();
            return View(new Seance());
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Seance seance)
        {
            var conflit = await _conflitService.VerifierConflits(seance);
            if (conflit.AConflit && conflit.EstBloquant)
            {
                foreach (var msg in conflit.Messages)
                    ModelState.AddModelError("", msg);

                await RemplirListes();
                return View(seance);
            }

            seance.DateCreation = DateTime.Now;
            _context.Seances.Add(seance);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Séance créée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        private async Task RemplirListes()
        {
            // Mentions et Parcours (si utiles plus tard)
            ViewBag.Mentions = await _context.Mentions.ToListAsync();
            ViewBag.Parcours = await _context.Parcours.ToListAsync();

            // Matières : SelectList (Value = MatiereUEId, Text = Nom)
            var matieres = await _context.MatiereUEs.ToListAsync();
            ViewBag.Matieres = new SelectList(matieres, "MatiereUEId", "Nom");

            // Enseignants : SelectList avec nom complet
            var enseignants = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .Select(e => new
                {
                    e.EnseignantId,
                    NomComplet = e.Utilisateur != null ? $"{e.Utilisateur.Nom} {e.Utilisateur.Prenom}" : "Inconnu"
                })
                .ToListAsync();
            ViewBag.Enseignants = new SelectList(enseignants, "EnseignantId", "NomComplet");

            // Promotions
            var promotions = await _context.Promotions.ToListAsync();
            ViewBag.Promotions = new SelectList(promotions, "PromotionId", "Code");

            // Créneaux horaires (format HH:mm - HH:mm)
            var creneaux = await _context.CreneauHoraires.OrderBy(c => c.HeureDebut).ToListAsync();
            ViewBag.Creneaux = creneaux.Select(c => new SelectListItem
            {
                Value = c.CreneauHoraireId.ToString(),
                Text = $"{c.HeureDebut:hh\\:mm} - {c.HeureFin:hh\\:mm}"
            }).ToList();

            // Salles actives
            var salles = await _context.Salles.Include(s => s.Batiment)
                .Where(s => s.Statut == "Active").ToListAsync();
            ViewBag.Salles = new SelectList(salles, "SalleId", "Code");

            // Semestres actifs
            var semestres = await _context.Semestres.Where(s => s.Actif).ToListAsync();
            ViewBag.Semestres = new SelectList(semestres, "SemestreId", "Libelle");
        }
    }
}