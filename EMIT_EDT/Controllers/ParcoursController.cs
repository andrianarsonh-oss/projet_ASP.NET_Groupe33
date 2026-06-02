using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class ParcoursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParcoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var parcours = await _context.Parcours
                .Include(p => p.Mention)
                .Include(p => p.Promotions)
                .ToListAsync();
            return View(parcours);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Mentions = _context.Mentions.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Parcours parcours)
        {
            if (ModelState.IsValid)
            {
                _context.Parcours.Add(parcours);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Parcours créé !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Mentions = _context.Mentions.ToList();
            return View(parcours);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var parcours = await _context.Parcours.FindAsync(id);
            if (parcours == null) return NotFound();
            ViewBag.Mentions = _context.Mentions.ToList();
            return View(parcours);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int id, Parcours parcours)
        {
            if (id != parcours.ParcoursId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Parcours.Update(parcours);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Parcours modifié !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Mentions = _context.Mentions.ToList();
            return View(parcours);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var parcours = await _context.Parcours.FindAsync(id);
            if (parcours == null) return NotFound();
            _context.Parcours.Remove(parcours);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Parcours supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}