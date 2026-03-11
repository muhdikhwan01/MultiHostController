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
}
