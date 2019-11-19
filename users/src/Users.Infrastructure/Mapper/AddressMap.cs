using FluentNHibernate.Mapping;
using Users.Domain.Common;

namespace Users.Infrastructure.Mapper
{
    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Schema("public");
            Table("Addresses");

            Id(x => x.Id)
                .GeneratedBy.Guid();

            Map(x => x.Line)
                .Length(100)
                .Not.Nullable();

            Map(x => x.Number)
                .Not.Nullable();

            Map(x => x.PostCode)
                .Length(10)
                .Not.Nullable();

            References(x => x.User)
                .LazyLoad()
                .Column("UserId");
        }
    }
}