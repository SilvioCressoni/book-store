using FluentNHibernate.Mapping;
using Users.Domain.Common;

namespace Users.Infrastructure.Mapper
{
    public class PhoneMap : ClassMap<Phone>
    {
        public PhoneMap()
        {
            Schema("public");
            Table("Phones");
            
            Id(x => x.Id)
                .GeneratedBy.Guid();

            Map(x => x.Number)
                .Not.Nullable()
                .Length(15);

            References(x => x.User)
                .LazyLoad()
                .Column("UserId");
        }
    }
}