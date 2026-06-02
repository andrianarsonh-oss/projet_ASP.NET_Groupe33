using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EMIT_EDT.Services;
using EMIT_EDT.Data;
using Microsoft.EntityFrameworkCore;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly StatistiqueService _statService;

        public DashboardController(ApplicationDbContext context, StatistiqueService statService)
        {
            _context = context;
            _statService = statService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalSeances = await _statService.GetTotalSeancesActives();
            ViewBag.TotalSalles = await _statService.GetTotalSalles();
            ViewBag.SallesOccupees = await _statService.GetSallesOccupees();
            ViewBag.SallesMaintenance = await _statService.GetSallesMaintenance();
            ViewBag.IncidentsOuverts = await _statService.GetTotalIncidentsOuverts();
            ViewBag.Mentions = await _context.Mentions.ToListAsync();
            ViewBag.Parcours = await _context.Parcours.Include(p => p.Mention).ToListAsync();

            return View();
        }
    }
}