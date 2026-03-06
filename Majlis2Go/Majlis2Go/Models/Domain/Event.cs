// Models/Domain/Event.cs
using System;
using System.Collections.Generic;

namespace Majlis2Go.Models.Domain
{
    public class Event
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string Title { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Location { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
