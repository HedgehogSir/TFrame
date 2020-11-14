using UnityEngine;

namespace TFrame
{
    public class SingletonExample : MonoBehaviour
    {
        private void Start()
        {
            Class2Singleton.Instance.Log("Hello World!");
            
            //释放掉该单例
            Class2Singleton.Instance.Dispose();

            //创建一个不同的单例
            Class2Singleton.Instance.Log("Hello World!");

            Class2SignetonProperty.Instance.Log("Hello World!");
        }
    }

    /// <summary>
    /// 通过继承类实现单例
    /// 优点是写法简单
    /// 缺点是必须继承指定父类
    /// </summary>
    class Class2Singleton : Singleton<Class2Singleton>
    {
        private Class2Singleton() { }

        public override void OnSingletonInit() { }

        public void Log(string content)
        {
            Debug.LogFormat($"<color=#00ffffff>Class2Singleton {GetHashCode()} {content}</color>");
        }
    }

    /// <summary>
    /// 通过继承接口实现单例
    /// 优点是不限制父类继承
    /// 缺点是实现稍微复杂一些
    /// </summary>
    class Class2SignetonProperty : ISingleton
    {
        public static Class2SignetonProperty Instance
        {
            get { return SingletonProperty<Class2SignetonProperty>.Instance; }
        }

        private Class2SignetonProperty() { }
        public void OnSingletonInit() { }

        public void Dispose()
        {
            SingletonProperty<Class2SignetonProperty>.Dispose();
        }

        public void Log(string content)
        {
            Debug.LogFormat($"<color=#00ffffff>Class2SignetonProperty {GetHashCode()} {content}</color>");
        }
    }
}