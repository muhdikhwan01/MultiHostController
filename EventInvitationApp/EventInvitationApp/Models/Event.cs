using System.ComponentModel.DataAnnotations;

namespace EventInvitationApp.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event title is required")]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Host name is required")]
        [StringLength(50)]
        public string HostName { get; set; }

        // Navigation property
        public List<RSVP> RSVPs { get; set; } = new();
    }
}
