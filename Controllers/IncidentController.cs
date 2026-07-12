using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class IncidentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var incidents = await _context.Incidents
                .Include(i => i.Salle).ThenInclude(s => s!.Batiment)
                .OrderByDescending(i => i.DateSignalement)
                .ToListAsync();
            return View(incidents);
        }

        public IActionResult Create()
        {
            ViewBag.Salles = _context.Salles.Include(s => s.Batiment).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Incident incident)
        {
            if (ModelState.IsValid)
            {
                incident.DateSignalement = DateTime.Now;
                incident.Statut = "Signale";
                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Incident signalé !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Salles = _context.Salles.Include(s => s.Batiment).ToList();
            return View(incident);
        }
    }
}