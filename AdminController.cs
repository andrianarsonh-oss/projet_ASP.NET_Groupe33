using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE
        public async Task<IActionResult> Index()
        {
            var admins = await _context.Admins.ToListAsync();
            return View(admins);
        }

        // AJOUT (GET)
        public IActionResult Create()
        {
            return View();
        }

        // AJOUT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin administrateur)
        {
            if (ModelState.IsValid)
            {
                _context.Admins.Add(administrateur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Administrateur créé.";
                return RedirectToAction(nameof(Index));
            }
            return View(administrateur);
        }

        // MODIFICATION (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var administrateur = await _context.Admins.FindAsync(id);
            if (administrateur == null) return NotFound();
            return View(administrateur);
        }

        // MODIFICATION (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Admin administrateur)
        {
            if (id != administrateur.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Admins.Update(administrateur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Administrateur modifié.";
                return RedirectToAction(nameof(Index));
            }
            return View(administrateur);
        }

        // SUPPRESSION
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var administrateur = await _context.Admins.FindAsync(id);
            if (administrateur == null) return NotFound();
            _context.Admins.Remove(administrateur);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Administrateur supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}