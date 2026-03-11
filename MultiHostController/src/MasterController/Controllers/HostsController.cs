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

    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private static List<DeploymentTask> tasks = new();

        [HttpPost]
        public IActionResult CreateTask(DeploymentTask task)
        {
            task.Id = tasks.Count + 1;
            tasks.Add(task);

            return Ok(task);
        }

        [HttpGet("{hostId}")]
        public IActionResult GetTasksForHost(int hostId)
        {
            var pending = tasks
                .Where(t => t.HostId == hostId && t.Status == "Pending")
                .ToList();

            return Ok(pending);
        }

        [HttpPost("complete/{id}")]
        public IActionResult CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.Status = "Completed";

            return Ok();
        }
    }
}
