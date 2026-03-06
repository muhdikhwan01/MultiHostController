using FluentNHibernate.Mapping;
using Majlis2Go.Models;

namespace Majlis2Go.Mappings
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customers");
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.FirstName).Not.Nullable().Length(100);
            Map(x => x.LastName).Not.Nullable().Length(100);
            Map(x => x.Email).Not.Nullable().Length(256);
            Map(x => x.Phone).Length(40);
            // Add relationships later, e.g. HasMany(x => x.Events).Cascade.All().Inverse();
        }
    }
}
