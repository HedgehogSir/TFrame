using System;
using System.Collections.Generic;

namespace TFrame
{
    public class TypeEventSystem
    {
        /// <summary>
        /// 字典类无法保存泛型类，于是用这个接口作为中转
        /// </summary>
        interface IRegisteration {}

        class Registeration<T> : IRegisteration
        {
            /// <summary>
            /// 监听的事件回调
            /// </summary>
            public Action<T> OnReceives = obj => { };
        }

        private static Dictionary<Type, IRegisteration> mTypeEventDict = new Dictionary<Type, IRegisteration>();

        /// <summary>
        /// 注册事件
        /// </summary>
        public static void Register<T>(System.Action<T> onReceive)
        {
            var type = typeof(T);

            if (mTypeEventDict.TryGetValue(type, out var registeration))
            {
                var reg = registeration as Registeration<T>;
                reg.OnReceives += onReceive;
            }
            else
            {
                var reg = new Registeration<T>();
                reg.OnReceives += onReceive;
                mTypeEventDict.Add(type, reg);
            }
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        public static void UnRegister<T>(System.Action<T> onReceive)
        {
            var type = typeof(T);

            if (mTypeEventDict.TryGetValue(type, out var registeration))
            {
                var reg = registeration as Registeration<T>;
                reg.OnReceives -= onReceive;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        public static void Send<T>(T t)
        {
            var type = typeof(T);

            if (mTypeEventDict.TryGetValue(type, out var registeration))
            {
                var reg = registeration as Registeration<T>;
                reg.OnReceives(t);
            }
        }

        /// <summary>
        /// 直接从对象池分配一个出来默认类型
        /// 适用于没有参数的事件类型，但是需要继承 IPoolable
        /// </summary>
        public static void Send<T>() where T : IPoolable, new()
        {
            var type = typeof(T);

            if (mTypeEventDict.TryGetValue(type, out var registeration))
            {
                var reg = registeration as Registeration<T>;
                var data = SafeObjectPool<T>.Instance.Allocate();
                reg.OnReceives(data);
                SafeObjectPool<T>.Instance.Recycle(data);
            }
        }

        public static void Dispose()
        {
            mTypeEventDict.Clear();
        }
    }
}