using NHibernate;

namespace Majlis2Go.Models
{
    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; } = string.Empty;
        public virtual string LastName { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string Phone { get; set; } = string.Empty;
        public virtual IList<EventDetail> Events { get; set; } = new List<EventDetail>();
    }
}
