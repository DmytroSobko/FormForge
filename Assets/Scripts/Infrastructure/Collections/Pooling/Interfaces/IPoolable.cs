namespace FormForge.Collections
{
    public interface IPoolable
    {
        void Init(AbstractPool owner);
        void OnAllocate();
        void OnDeallocate();
        void Destroy();
    }
}