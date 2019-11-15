using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using User.Domain.Common;

namespace User.Infrastructure.Mapper
{
    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Table(nameof(Address));

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