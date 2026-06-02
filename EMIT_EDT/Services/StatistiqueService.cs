using EMIT_EDT.Data;
using Microsoft.EntityFrameworkCore;

namespace EMIT_EDT.Services
{
    public class StatistiqueService
    {
        private readonly ApplicationDbContext _context;

        public StatistiqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalSeancesActives()
        {
            return await _context.Seances.CountAsync(s => s.Statut == "Actif");
        }

        public async Task<int> GetTotalSalles()
        {
            return await _context.Salles.CountAsync();
        }

        public async Task<int> GetSallesOccupees()
        {
            return await _context.Seances
                .Where(s => s.Statut == "Actif")
                .Select(s => s.SalleId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetSallesMaintenance()
        {
            return await _context.Salles.CountAsync(s => s.Statut == "EnMaintenance");
        }

        public async Task<int> GetTotalIncidentsOuverts()
        {
            return await _context.Incidents.CountAsync(i => i.Statut != "Resolu");
        }
    }
}