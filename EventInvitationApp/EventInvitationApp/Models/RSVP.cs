using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EventInvitationApp.Resources;

namespace EventInvitationApp.Models
{
    public class RSVP
    {
        public int RSVPId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Labels), ErrorMessageResourceName = nameof(Labels.GuestIsRequired))]
        [Display(ResourceType = typeof(Labels), Name = nameof(Labels.GuestIsRequired))]
        [StringLength(50)]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Please select attending status")]
        public bool? IsAttending { get; set; }

        // Foreign key
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
