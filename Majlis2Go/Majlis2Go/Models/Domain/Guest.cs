// Models/Domain/Guest.cs    (optional if separate from Invitation)
namespace Majlis2Go.Models.Domain
{
    public class Guest
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
    }
}
