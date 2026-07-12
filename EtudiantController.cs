using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class EtudiantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EtudiantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE
        public async Task<IActionResult> Index()
        {
            var etudiants = await _context.Etudiants
                .Include(e => e.Avoirs).ThenInclude(a => a.Parcours)
                .ToListAsync();
            return View(etudiants);
        }

        // AJOUT (GET)
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Parcours = new SelectList(_context.Parcours, "ParcoursId", "Nom");
            return View();
        }

        // AJOUT (POST)
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Etudiant etudiant)
        {
            if (ModelState.IsValid)
            {
                _context.Etudiants.Add(etudiant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Étudiant créé.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parcours = new SelectList(_context.Parcours, "ParcoursId", "Nom");
            return View(etudiant);
        }

        // MODIFICATION (GET) – UNIQUE
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant == null) return NotFound();
            return View(etudiant);
        }

        // MODIFICATION (POST) – UNIQUE, avec gestion du mot de passe
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Etudiant etudiant)
        {
            if (id != etudiant.Id) return NotFound();

            // Ignorer la validation du champ MotPasse s'il n'est pas modifié
            ModelState.Remove("MotPasse");

            if (ModelState.IsValid)
            {
                var existing = await _context.Etudiants.FindAsync(id);
                if (existing == null) return NotFound();

                // Mettre à jour uniquement les champs modifiables
                existing.Matricule = etudiant.Matricule;
                existing.Nom = etudiant.Nom;
                existing.Prenom = etudiant.Prenom;
                existing.Email = etudiant.Email;
                existing.NumCin = etudiant.NumCin;
                existing.StatutValide = etudiant.StatutValide;

                // Si un nouveau mot de passe est fourni (facultatif), on le met à jour
                if (!string.IsNullOrWhiteSpace(etudiant.MotPasse))
                {
                    existing.MotPasse = etudiant.MotPasse;
                }
                // Sinon, on conserve l'ancien mot de passe

                await _context.SaveChangesAsync();
                TempData["Success"] = "Étudiant modifié.";
                return RedirectToAction(nameof(Index));
            }

            return View(etudiant);
        }

        // SUPPRESSION
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant == null) return NotFound();
            _context.Etudiants.Remove(etudiant);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Étudiant supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}