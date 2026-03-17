namespace Game339.Shared.DependencyInjection
{
    public interface IMiniContainer
    {
        T Resolve<T>();
    }
}