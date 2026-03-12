using Microsoft.AspNetCore.Mvc;
using MasterController.Models;
using MasterController.Data;

namespace MasterController.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        // Database context
        private readonly AppDbContext _context;

        // Constructor injection
        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        //----------------------------------------------------------
        // Create new deployment task
        //----------------------------------------------------------
        [HttpPost]
        public IActionResult CreateTask(DeploymentTask task)
        {
            task.Status = "Pending";
            task.CreatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return Ok(task);
        }

        //----------------------------------------------------------
        // Get tasks for a specific host
        //----------------------------------------------------------
        [HttpGet("{hostId}")]
        public IActionResult GetTasksForHost(int hostId)
        {
            var pendingTasks = _context.Tasks
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
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.Status = "Completed";

            _context.SaveChanges();

            return Ok(task);
        }

        //----------------------------------------------------------
        // Receive result from ClientAgent
        //----------------------------------------------------------
        [HttpPost("result")]
        public IActionResult SubmitResult(TaskResult result)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == result.TaskId);

            if (task == null)
                return NotFound();

            task.Status = result.Status;

            _context.SaveChanges();

            Console.WriteLine($"Task {task.Id} finished with status: {result.Status}");

            return Ok();
        }
    }
}