namespace MasterController.Models
{
    public class TaskResult
    {
        public int TaskId { get; set; }

        public string Status { get; set; }

        public string Output { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}