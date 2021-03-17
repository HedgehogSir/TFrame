using UnityEngine;

namespace TFrame
{
    public class MonoSingletonExample : MonoBehaviour
    {
        private void Start()
        {
            Class2MonoSingleton.Instance.Log("创建并使用 Class2MonoSingleton 单例");
            Class2MonoSingletonPath.Instance.Log("创建并使用 Class2MonoSingletonPath 单例");
        }
    }

    class Class2MonoSingleton : MonoSingleton<Class2MonoSingleton>
    {
        public override void OnSingletonInit()
        {
            Debug.Log(this.name + " OnSingletonInit");
        }

        private void Awake()
        {
            Debug.Log(this.name + " Awake");
        }

        private void Start()
        {
            Debug.Log(this.name + " Start");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Debug.Log(this.name + " OnDestroy");
        }

        public void Log(string content)
        {
            Debug.LogFormat($"<color=#00ffffff>HashCode:{GetHashCode()} {content}</color>");
        }
    }

    [MonoSingletonPath("TFrame/MonoSingeltonPath")]
    class Class2MonoSingletonPath : MonoSingleton<Class2MonoSingletonPath>
    {
        public void Log(string content)
        {
            Debug.LogFormat($"<color=#00ffffff>HashCode:{GetHashCode()} {content}</color>");
        }
    }
}