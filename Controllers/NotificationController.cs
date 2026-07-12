using Microsoft.AspNetCore.Mvc;
using EMIT_EDT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EMIT_EDT.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly NotificationService _notifService;

        public NotificationController(NotificationService notifService)
        {
            _notifService = notifService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifs = await _notifService.GetNotifications(userId!, 50);
            return View(notifs);
        }
    }
}