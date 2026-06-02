using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class SalleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE – avec recherche, localisation et équipements
        public async Task<IActionResult> Index(string? searchString)
        {
            ViewBag.SearchString = searchString;  // pour réafficher la valeur dans le champ

            var query = _context.Salles
                .Include(s => s.Batiment)
                .Include(s => s.TypeSalle)
                .Include(s => s.SalleEquipements)!      // ⬅️ charge les équipements
                    .ThenInclude(se => se.Equipement)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var search = searchString.Trim().ToLower();
                query = query.Where(s =>
                    s.Code.ToLower().Contains(search) ||
                    s.Nom.ToLower().Contains(search) ||
                    s.PositionSalle!.ToLower().Contains(search) ||
                    s.Batiment!.Nom.ToLower().Contains(search) ||
                    s.TypeSalle!.Libelle.ToLower().Contains(search) ||
                    // recherche parmi les équipements
                    s.SalleEquipements!.Any(se => se.Equipement!.Nom.ToLower().Contains(search))
                );
            }

            var salles = await query
                .OrderBy(s => s.Batiment!.Nom)
                .ThenBy(s => s.Code)
                .ToListAsync();

            return View(salles);
        }

        // DÉTAIL
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var salle = await _context.Salles
                .Include(s => s.Batiment)
                .Include(s => s.TypeSalle)
                .Include(s => s.SalleEquipements)!.ThenInclude(se => se.Equipement)
                .FirstOrDefaultAsync(s => s.SalleId == id);
            if (salle == null) return NotFound();
            return View(salle);
        }

        // CRÉER (GET)
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Batiments = _context.Batiments.ToList();
            ViewBag.TypeSalles = _context.TypeSalles.ToList();
            ViewBag.Equipements = _context.Equipements.ToList();   // ⬅️ pour un éventuel formulaire futur
            return View();
        }

        // CRÉER (POST)
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Salle salle)
        {
            if (ModelState.IsValid)
            {
                _context.Salles.Add(salle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Salle créée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Batiments = _context.Batiments.ToList();
            ViewBag.TypeSalles = _context.TypeSalles.ToList();
            return View(salle);
        }

        // MODIFIER (GET)
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var salle = await _context.Salles
                .Include(s => s.Batiment)
                .Include(s => s.TypeSalle)
                .Include(s => s.SalleEquipements)!.ThenInclude(se => se.Equipement)
                .FirstOrDefaultAsync(s => s.SalleId == id);
            if (salle == null) return NotFound();
            ViewBag.Batiments = _context.Batiments.ToList();
            ViewBag.TypeSalles = _context.TypeSalles.ToList();
            ViewBag.Equipements = _context.Equipements.ToList();
            return View(salle);
        }

        // MODIFIER (POST)
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int id, Salle salle)
        {
            if (id != salle.SalleId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Salles.Update(salle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Salle modifiée !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Batiments = _context.Batiments.ToList();
            ViewBag.TypeSalles = _context.TypeSalles.ToList();
            return View(salle);
        }

        // SUPPRIMER
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null) return NotFound();
            _context.Salles.Remove(salle);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Salle supprimée.";
            return RedirectToAction(nameof(Index));
        }
    }
}