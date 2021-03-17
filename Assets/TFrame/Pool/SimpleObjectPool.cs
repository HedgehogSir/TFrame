using System;

namespace TFrame
{
    public class SimpleObjectPool<T> : Pool<T>
    {
        readonly Action<T> mRecycleMethod;

        public SimpleObjectPool(Func<T> factoryMethod, Action<T> recycleMethod = null,int initCount = 0)
        {
            mFactory = new CustomObjectFactory<T>(factoryMethod);
            mRecycleMethod = recycleMethod;

            for (int i = 0; i < initCount; i++)
            {
                mCacheStack.Push(mFactory.Create());
            }
        }

        public override bool Recycle(T obj)
        {
            if (mRecycleMethod != null)
            {
                mRecycleMethod.Invoke(obj);
            }
            
            mCacheStack.Push(obj);
            return true;
        }
    }
}