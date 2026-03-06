// Models/Mappings/EventMap.cs
using FluentNHibernate.Mapping;
using Majlis2Go.Models.Domain;

namespace Majlis2Go.Models.Mappings
{
    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("Events");
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title).Not.Nullable().Length(200);
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
            Map(x => x.Location).Length(500);
            Map(x => x.Description).Length(2000);
            HasMany(x => x.Invitations)
                .Cascade.All()
                .Inverse()
                .KeyColumn("EventId");
        }
    }
}
