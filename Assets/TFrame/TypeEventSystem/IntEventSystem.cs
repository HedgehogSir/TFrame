using System.Collections.Generic;

namespace TFrame
{
    public class IntEventSystem
    {
        class Listener
        {
            private LinkedList<OnEvent> mEventList;

            public void Invoke(params object[] param)
            {
                if (mEventList == null)
                {
                    return;
                }

                foreach (var item in mEventList)
                {
                    item?.Invoke(param);
                }
            }

            public void Add(OnEvent listener)
            {
                if (mEventList == null)
                {
                    mEventList = new LinkedList<OnEvent>();
                }

                mEventList.AddLast(listener);
            }

            public void Remove(OnEvent listener)
            {
                if (mEventList == null)
                {
                    return;
                }

                mEventList.Remove(listener);
            }

            public void RemoveAll()
            {
                if (mEventList == null)
                {
                    return;
                }

                mEventList.Clear();
            }
        }

        private static readonly Dictionary<int, Listener> mListenerDict = new Dictionary<int, Listener>(50);

        public static void Register(int key, OnEvent fun)
        {
            if (!mListenerDict.TryGetValue(key, out var listener))
            {
                listener = new Listener();
                mListenerDict.Add(key, listener);
            }

            listener.Add(fun);
        }

        public static void UnRegister(int key, OnEvent fun)
        {
            if (mListenerDict.TryGetValue(key, out var listener))
            {
                listener.Remove(fun);
            }
        }

        public static void UnRegisterAll(int key)
        {
            if (mListenerDict.TryGetValue(key, out var listener))
            {
                listener.RemoveAll();
                listener = null;

                mListenerDict.Remove(key);
            }
        }

        public static void Send(int key, params object[] param)
        {
            if (mListenerDict.TryGetValue(key, out var listener))
            {
                listener.Invoke(param);
            }
        }
    }

    public delegate void OnEvent(params object[] param);
}
