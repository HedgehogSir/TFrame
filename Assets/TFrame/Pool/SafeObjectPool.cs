using System;

namespace TFrame
{
    public class SafeObjectPool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {
        #region Singleton
        void ISingleton.OnSingletonInit() {}

        protected SafeObjectPool()
        {
            mFactory = new DefaultObjectFactory<T>();
        }

        public static SafeObjectPool<T> Instance
        {
            get { return SingletonProperty<SafeObjectPool<T>>.Instance; }
        }

        public void Dispose()
        {
            SingletonProperty<SafeObjectPool<T>>.Dispose();
        }
        #endregion

        public void Init(int maxCount, int initCount)
        {
            MaxCacheCount = maxCount;

            initCount = Math.Min(maxCount, initCount);

            if (CurCount < initCount)
            {
                for (var i = CurCount; i < initCount; ++i)
                {
                    Recycle(mFactory.Create());
                }
            }
        }

        public int MaxCacheCount
        {
            get { return mMaxCount; }
            set
            {
                mMaxCount = Math.Max(1, value);

                if (mMaxCount < mCacheStack.Count)
                {
                    int removeCount = mCacheStack.Count - mMaxCount;
                    while (removeCount > 0)
                    {
                        mCacheStack.Pop();
                        removeCount--;
                    }
                }
            }
        }

        public override T Allocate()
        {
            var result = base.Allocate();
            result.IsRecycled = false;
            return result;
        }

        public override bool Recycle(T t)
        {
            if (t == null || t.IsRecycled)
            {
                return false;
            }

            if (mCacheStack.Count >= mMaxCount)
            {
                t.OnRecycled();
                return false;
            }

            t.IsRecycled = true;
            t.OnRecycled();
            mCacheStack.Push(t);

            return true;
        }
    }
}