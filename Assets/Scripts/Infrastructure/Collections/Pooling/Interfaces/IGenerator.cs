namespace FormForge.Infrastructure.Collections
{
    public interface IGenerator
    {
        IPoolable CreateInstance();
    }
}
