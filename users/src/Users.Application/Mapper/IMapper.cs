namespace Users.Application.Mapper
{
    public interface IMapper<TSource,TDestiny>
    {
        TDestiny Map(TSource source);
    }
}