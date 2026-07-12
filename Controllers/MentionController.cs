using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class MentionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MentionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mention
        public async Task<IActionResult> Index()
        {
            var mentions = await _context.Mentions.Include(m => m.Parcours).ToListAsync();
            return View(mentions);
        }

        // GET: Mention/Create
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mention/Create
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Mention mention)
        {
            if (ModelState.IsValid)
            {
                _context.Mentions.Add(mention);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Mention créée !";
                return RedirectToAction(nameof(Index));
            }
            return View(mention);
        }

        // GET: Mention/Edit/5
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var mention = await _context.Mentions.FindAsync(id);
            if (mention == null) return NotFound();
            return View(mention);
        }

        // POST: Mention/Edit/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int id, Mention mention)
        {
            if (id != mention.MentionId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Mentions.Update(mention);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Mention modifiée !";
                return RedirectToAction(nameof(Index));
            }
            return View(mention);
        }

        // GET: Mention/Delete/5
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var mention = await _context.Mentions.FindAsync(id);
            if (mention == null) return NotFound();
            _context.Mentions.Remove(mention);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Mention supprimée.";
            return RedirectToAction(nameof(Index));
        }
    }
}