using FluentNHibernate.Mapping;

public class EventDetailMap : ClassMap<EventDetail>
{
    public EventDetailMap()
    {
        Table("Events");
        Id(x => x.Id).GeneratedBy.GuidComb();
        References(x => x.Customer).Column("CustomerId").Not.Nullable();
        Map(x => x.Title).Not.Nullable();
        Map(x => x.EventDate).Not.Nullable();
        HasMany(x => x.Guests).Cascade.All().Inverse();
    }
}
