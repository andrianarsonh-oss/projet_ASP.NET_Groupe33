using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class PromotionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var promotions = await _context.Promotions
                .Include(p => p.Parcours).ThenInclude(p => p!.Mention)
                .Include(p => p.Groupes)
                .OrderBy(p => p.Parcours!.Mention!.Nom)
                .ThenBy(p => p.Niveau)
                .ToListAsync();
            return View(promotions);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Parcours = _context.Parcours.Include(p => p.Mention).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                _context.Promotions.Add(promotion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Promotion créée !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parcours = _context.Parcours.Include(p => p.Mention).ToList();
            return View(promotion);
        }
    }
}