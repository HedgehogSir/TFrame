using System.Collections.Generic;

namespace TFrame
{
    public abstract class Pool<T> : IPool<T>
    {
        public int CurCount
        {
            get { return mCacheStack.Count; }
        }

        protected IObjectFactory<T> mFactory;

        protected readonly Stack<T> mCacheStack = new Stack<T>();

        protected int mMaxCount = 12;

        public virtual T Allocate()
        {
            return mCacheStack.Count == 0
                ? mFactory.Create()
                : mCacheStack.Pop();
        }

        public abstract bool Recycle(T obj);
    }
}