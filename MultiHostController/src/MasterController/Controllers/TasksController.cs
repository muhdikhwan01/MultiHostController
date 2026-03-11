using Microsoft.AspNetCore.Mvc;
using MasterController.Models;

namespace MasterController.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        // In-memory task storage (for this assessment)
        private static List<DeploymentTask> tasks = new();

        //----------------------------------------------------------
        // Create new deployment task
        //----------------------------------------------------------
        [HttpPost]
        public IActionResult CreateTask(DeploymentTask task)
        {
            task.Id = tasks.Count + 1;
            task.Status = "Pending";
            task.CreatedAt = DateTime.UtcNow;

            tasks.Add(task);

            return Ok(task);
        }

        //----------------------------------------------------------
        // Get tasks for a specific host
        // Only return tasks that are still Pending
        //----------------------------------------------------------
        [HttpGet("{hostId}")]
        public IActionResult GetTasksForHost(int hostId)
        {
            var pendingTasks = tasks
                .Where(t => t.HostId == hostId && t.Status == "Pending")
                .ToList();

            return Ok(pendingTasks);
        }

        //----------------------------------------------------------
        // Mark task as completed
        //----------------------------------------------------------
        [HttpPost("complete/{id}")]
        public IActionResult CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.Status = "Completed";

            return Ok(task);
        }
    }
}