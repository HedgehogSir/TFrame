using System;
using System.Linq;
using System.Reflection;
#if UNITY_5_6_OR_NEWER
using UnityEngine;
using Object = UnityEngine.Object;
#endif

namespace TFrame
{
#if UNITY_5_6_OR_NEWER
	public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
	{
		protected static T mInstance;

		public static T Instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = MonoSingletonCreator.CreateMonoSingleton<T>();
				}

				return mInstance;
			}
		}

		public virtual void OnSingletonInit()
		{
		}

		public virtual void Dispose()
		{
			Destroy(gameObject);
		}

		protected virtual void OnDestroy()
		{
			mInstance = null;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class MonoSingletonPath : Attribute
	{
		public string PathInHierarchy { get; private set; }

		public MonoSingletonPath(string pathInHierarchy)
		{
			PathInHierarchy = pathInHierarchy;
		}

	}

	public static class MonoSingletonCreator
	{
		public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
		{
			T instance = Object.FindObjectOfType<T>();

			if (instance != null)
			{
				instance.OnSingletonInit();
				return instance;
			}

			MemberInfo info = typeof(T);
			var attributes = info.GetCustomAttributes(true);
			foreach (var atribute in attributes)
			{
				var defineAttri = atribute as MonoSingletonPath;
                if (defineAttri != null)
                {
					var path = defineAttri.PathInHierarchy;
                    if (string.IsNullOrEmpty(path))
                    {
						break;
                    }
					var subPathList = path.Split('/').Where(p => !string.IsNullOrEmpty(p)).ToList();
					var root = GameObject.Find(subPathList[0]);
					if (root == null)
					{
						root = new GameObject(subPathList[0]);
						Object.DontDestroyOnLoad(root);
					}
					var node = root.transform;
					var index = 1;
                    while (subPathList.Count > index)
                    {
						var child = node.Find(subPathList[index]);
                        if (child == null)
                        {
							child = new GameObject(subPathList[index]).transform;
							child.SetParent(node);
                        }
						node = child;
						index++;
					}
					instance = node.gameObject.AddComponent<T>();
				}
			}

			if (instance == null)
			{
				var obj = new GameObject(typeof(T).Name);
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<T>();
			}

			instance.OnSingletonInit();
			return instance;
		}
	}
#endif
}
