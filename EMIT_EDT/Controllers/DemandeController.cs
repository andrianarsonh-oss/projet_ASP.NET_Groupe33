using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class DemandeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DemandeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE
        public async Task<IActionResult> Index()
        {
            var demandes = await _context.Demandes
                .Include(d => d.Etudiant)
                .Include(d => d.Salle)
                .Include(d => d.Traitements).ThenInclude(t => t.Admin)
                .OrderByDescending(d => d.DureDebut)
                .ToListAsync();
            return View(demandes);
        }

        // CRÉER (GET)
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Etudiants = new SelectList(_context.Etudiants, "Id", "Nom");
            ViewBag.Salles = new SelectList(_context.Salles, "SalleId", "Code");
            return View();
        }

        // CRÉER (POST)
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Demande demande)
        {
            if (ModelState.IsValid)
            {
                _context.Demandes.Add(demande);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Demande créée avec succès.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Etudiants = new SelectList(_context.Etudiants, "Id", "Nom");
            ViewBag.Salles = new SelectList(_context.Salles, "SalleId", "Code");
            return View(demande);
        }

        // MODIFIER (GET)
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var demande = await _context.Demandes.FindAsync(id);
            if (demande == null) return NotFound();

            ViewBag.Etudiants = new SelectList(_context.Etudiants, "Id", "Nom", demande.EtudiantId);
            ViewBag.Salles = new SelectList(_context.Salles, "SalleId", "Code", demande.SalleId);
            return View(demande);
        }

        // MODIFIER (POST)
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Demande demande)
        {
            if (id != demande.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Demandes.Update(demande);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Demande modifiée.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Etudiants = new SelectList(_context.Etudiants, "Id", "Nom", demande.EtudiantId);
            ViewBag.Salles = new SelectList(_context.Salles, "SalleId", "Code", demande.SalleId);
            return View(demande);
        }

        // SUPPRIMER
        [Authorize(Roles = "Admin,SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var demande = await _context.Demandes.FindAsync(id);
            if (demande == null) return NotFound();

            _context.Demandes.Remove(demande);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Demande supprimée.";
            return RedirectToAction(nameof(Index));
        }
    }
}