using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class BatimentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatimentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var batiments = await _context.Batiments.Include(b => b.Salles).ToListAsync();
            return View(batiments);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Batiment batiment)
        {
            if (ModelState.IsValid)
            {
                _context.Batiments.Add(batiment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Bâtiment créé !";
                return RedirectToAction(nameof(Index));
            }
            return View(batiment);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var batiment = await _context.Batiments.FindAsync(id);
            if (batiment == null) return NotFound();
            return View(batiment);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int id, Batiment batiment)
        {
            if (id != batiment.BatimentId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Batiments.Update(batiment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Bâtiment modifié !";
                return RedirectToAction(nameof(Index));
            }
            return View(batiment);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var batiment = await _context.Batiments.FindAsync(id);
            if (batiment == null) return NotFound();
            _context.Batiments.Remove(batiment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Bâtiment supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}