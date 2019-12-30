using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Users.Infrastructure.Mapper
{
    public class UserMap : ClassMap<Domain.Common.User>
    {
        public UserMap()
        {
            Schema("public");
            Table("Users");

            Id(x => x.Id)
                .GeneratedBy.Guid();

            Map(x => x.FirstName)
                .Length(20)
                .Not.Nullable();

            Map(x => x.LastNames)
                .Length(100)
                .Not.Nullable();

            Map(x => x.Email)
                .Length(100)
                .Not.Nullable()
                .Unique();

            Map(x => x.BirthDay)
                .Not.Nullable()
                .CustomType<UtcDateTimeType>();

            HasMany(x => x.Addresses)
                .LazyLoad()
                .Inverse()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.Phones)
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();
        }
    }
}