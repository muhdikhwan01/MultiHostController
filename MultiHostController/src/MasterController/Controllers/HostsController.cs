using Microsoft.AspNetCore.Mvc;
using MasterController.Models;
using MasterController.Data;

namespace MasterController.Controllers
{
    [ApiController]
    [Route("api/hosts")]
    public class HostsController : ControllerBase
    {
        // Database context
        private readonly AppDbContext _context;

        // Constructor injection
        public HostsController(AppDbContext context)
        {
            _context = context;
        }

        // Register a new host
        [HttpPost("register")]
        public IActionResult RegisterHost(MasterController.Models.Host host)
        {
            host.LastHeartbeat = DateTime.UtcNow;

            _context.Hosts.Add(host);
            _context.SaveChanges();

            return Ok(host);
        }

        // Get all hosts
        [HttpGet]
        public IActionResult GetHosts()
        {
            var hosts = _context.Hosts.ToList();
            return Ok(hosts);
        }

        // Heartbeat update
        [HttpPost("heartbeat/{id}")]
        public IActionResult Heartbeat(int id)
        {
            var host = _context.Hosts.FirstOrDefault(h => h.Id == id);

            if (host == null)
                return NotFound();

            host.LastHeartbeat = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok();
        }
    }
}