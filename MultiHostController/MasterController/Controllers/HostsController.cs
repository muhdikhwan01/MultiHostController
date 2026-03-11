using Microsoft.AspNetCore.Mvc;
using MasterController.Models;

namespace MasterController.Controllers
{
    [ApiController]
    [Route("api/hosts")]
    public class HostsController : ControllerBase
    {
        private static List<Models.Host> hosts = new();

        [HttpPost("register")]
        public IActionResult RegisterHost(Models.Host host)
        {
            host.Id = hosts.Count + 1;
            host.LastHeartbeat = DateTime.UtcNow;

            hosts.Add(host);

            return Ok(host);
        }

        [HttpGet]
        public IActionResult GetHosts()
        {
            return Ok(hosts);
        }

        [HttpPost("heartbeat/{id}")]
        public IActionResult Heartbeat(int id)
        {
            var host = hosts.FirstOrDefault(h => h.Id == id);

            if (host == null)
                return NotFound();

            host.LastHeartbeat = DateTime.UtcNow;

            return Ok();
        }
    }
}
