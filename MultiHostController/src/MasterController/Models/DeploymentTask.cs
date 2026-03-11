namespace MasterController.Models
{
    public class DeploymentTask
    {
        public int Id { get; set; }

        public int HostId { get; set; }

        public string Command { get; set; }

        // Default status
        public string Status { get; set; } = "Pending";

        // Automatically set creation time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}