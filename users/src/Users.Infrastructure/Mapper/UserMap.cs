using FluentNHibernate.Mapping;

namespace Users.Infrastructure.Mapper
{
    public class UserMap : ClassMap<Domain.Common.User>
    {
        public UserMap()
        {
            Table(nameof(Domain.Common.User));

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
                .Not.Nullable();

            HasMany(x => x.Addresses)
                .LazyLoad();

            HasMany(x => x.Phones)
                .LazyLoad()
                .AsSet();
        }
    }
}