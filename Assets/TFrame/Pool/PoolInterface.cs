namespace TFrame
{
    public interface IPool<T>
    {
        T Allocate();

        bool Recycle(T obj);
    }

    public interface IPoolable
    {
        void OnRecycled();
        bool IsRecycled { get; set; }
    }

    public interface IPoolType
    {
        void Recycle2Cache();
    }
}
