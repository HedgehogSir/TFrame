using System;
using System.Reflection;

namespace TFrame
{
	public abstract class Singleton<T> : ISingleton where T : Singleton<T>
	{
		protected static T mInstance;

		static object mLock = new object();

		public static T Instance
		{
			get
			{
				lock (mLock)
				{
					if (mInstance == null)
					{
						mInstance = SingletonCreator.CreateSingleton<T>();
					}
				}

				return mInstance;
			}
		}

		public virtual void Dispose()
		{
			mInstance = null;
		}

		public virtual void OnSingletonInit()
		{
		}
	}

	public static class SingletonProperty<T> where T : class, ISingleton
	{
		private static T mInstance;
		private static readonly object mLock = new object();

		public static T Instance
		{
			get
			{
				lock (mLock)
				{
					if (mInstance == null)
					{
						mInstance = SingletonCreator.CreateSingleton<T>();
					}
				}

				return mInstance;
			}
		}

		public static void Dispose()
		{
			mInstance = null;
		}
	}

	public static class SingletonCreator
	{
		public static T CreateSingleton<T>() where T : class, ISingleton
		{
			//// 方式一通过反射
			//// 获取私有构造函数
			//var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			//// 获取无参构造函数
			//var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
			//if (ctor == null)
			//{
			//	throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
			//}
			//// 通过构造函数，创建实例
			//var retInstance = ctor.Invoke(null) as T;

			// 方式二 通过 Activator 创建实例
			var retInstance = System.Activator.CreateInstance(typeof(T), true) as T;
			if (retInstance == null)
			{
				throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
			}

			retInstance.OnSingletonInit();

			return retInstance;
		}
	}
}