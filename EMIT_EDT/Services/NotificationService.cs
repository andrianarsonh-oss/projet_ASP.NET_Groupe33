using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.EntityFrameworkCore;

namespace EMIT_EDT.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Envoyer(string destinataireId, string type, string message, string? lien = null)
        {
            _context.Notifications.Add(new Notification
            {
                DestinataireId = destinataireId,
                Type = type,
                Message = message,
                Lien = lien,
                DateEnvoi = DateTime.Now,
                Canal = "InApp"
            });
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotifications(string userId, int limite = 50)
        {
            return await _context.Notifications
                .Where(n => n.DestinataireId == userId)
                .OrderByDescending(n => n.DateEnvoi)
                .Take(limite)
                .ToListAsync();
        }

        public async Task MarquerLue(int id)
        {
            var n = await _context.Notifications.FindAsync(id);
            if (n != null)
            {
                n.Lu = true;
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Ajoutez cette méthode
        public async Task<int> GetCountNonLues(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.DestinataireId == userId && !n.Lu);
        }
    }
}