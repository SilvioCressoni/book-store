namespace Users.Domain
{
    public interface IState<T>
    {
        T Id { get; }
    }
}
