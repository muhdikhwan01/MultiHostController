// Models/Mappings/InvitationMap.cs
using FluentNHibernate.Mapping;
using Majlis2Go.Models.Domain;

namespace Majlis2Go.Models.Mappings
{
    public class InvitationMap : ClassMap<Invitation>
    {
        public InvitationMap()
        {
            Table("Invitations");
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.RecipientName).Not.Nullable().Length(200);
            Map(x => x.RecipientEmail).Not.Nullable().Length(200);
            Map(x => x.IsAttending).Not.Nullable();
            Map(x => x.Message).Length(2000);
            References(x => x.Event).Column("EventId").Not.Nullable();
        }
    }
}
