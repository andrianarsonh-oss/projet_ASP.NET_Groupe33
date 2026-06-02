using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class MatiereController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatiereController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var matieres = await _context.MatiereUEs
                .Include(m => m.Semestre)
                .Include(m => m.Parcours)
                .Include(m => m.Enseignants)
                .OrderBy(m => m.Code)
                .ToListAsync();
            return View(matieres);
        }

        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Semestres = _context.Semestres.ToList();
            ViewBag.Parcours = _context.Parcours.ToList();
            ViewBag.Enseignants = _context.Enseignants.Include(e => e.Utilisateur).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Create(MatiereUE matiere)
        {
            if (ModelState.IsValid)
            {
                _context.MatiereUEs.Add(matiere);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Matière créée !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Semestres = _context.Semestres.ToList();
            ViewBag.Parcours = _context.Parcours.ToList();
            ViewBag.Enseignants = _context.Enseignants.Include(e => e.Utilisateur).ToList();
            return View(matiere);
        }
    }
}