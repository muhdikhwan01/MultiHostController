namespace MasterController.Models
{
    public class Host
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public string OS { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
    public class DeploymentTask
    {
        public int Id { get; set; }

        public int HostId { get; set; }

        public string Command { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
