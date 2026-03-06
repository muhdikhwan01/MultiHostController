using FluentNHibernate.Mapping;

namespace EventInvitationApp.Mappings
{
    public class EventsMapcs : ClassMap<Events>
    {
        public EventsMapcs()
        {
            Table("Events");
            Id(x => x.EventId);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Date);
            Map(x => x.Location);
        }
    }
}
