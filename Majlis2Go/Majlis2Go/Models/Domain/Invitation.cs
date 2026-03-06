// Models/Domain/Invitation.cs
using System;

namespace Majlis2Go.Models.Domain
{
    public class Invitation
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string RecipientName { get; set; }
        public virtual string RecipientEmail { get; set; }
        public virtual bool IsAttending { get; set; }
        public virtual string Message { get; set; }
        public virtual Event Event { get; set; }   // many-to-one
    }
}
