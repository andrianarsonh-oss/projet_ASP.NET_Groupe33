using Microsoft.AspNetCore.Mvc;
using EMIT_EDT.Services;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    [Authorize(Roles = "SuperAdmin,RespEDT")]
    public class StatistiqueController : Controller
    {
        private readonly StatistiqueService _statService;

        public StatistiqueController(StatistiqueService statService)
        {
            _statService = statService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalSeances = await _statService.GetTotalSeancesActives();
            ViewBag.TotalSalles = await _statService.GetTotalSalles();
            ViewBag.SallesOccupees = await _statService.GetSallesOccupees();
            ViewBag.SallesMaintenance = await _statService.GetSallesMaintenance();
            ViewBag.IncidentsOuverts = await _statService.GetTotalIncidentsOuverts();
            return View();
        }
    }
}