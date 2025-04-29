using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Controllers
{
    [Route("api/notify")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationConn _conn;
        public NotificationController(NotificationConn conn)
        {
            _conn = conn;
        }

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var nots = _conn.Notifications.ToList();
            return Ok(nots);
        }
    }
}
