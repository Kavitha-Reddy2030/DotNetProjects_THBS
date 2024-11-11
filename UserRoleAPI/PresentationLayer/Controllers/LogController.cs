using Microsoft.AspNetCore.Mvc;
using UserRoleAPI.Data;

namespace UserRoleAPI.PresentationLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly UserRoleDbContext _context;

        public LogController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _context.Logs.ToList();
            return Ok(logs);
        }
    }
}
