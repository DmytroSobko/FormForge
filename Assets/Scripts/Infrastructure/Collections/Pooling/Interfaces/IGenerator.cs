namespace FormForge.Collections
{
    public interface IGenerator
    {
        IPoolable CreateInstance();
    }
}
