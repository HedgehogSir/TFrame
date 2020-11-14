using UnityEngine;

namespace TFrame
{
    public class MonoSingletonExample : MonoBehaviour
    {
        private void Start()
        {
            Class2MonoSingleton.Instance.Log("Hello World!");
            Class2MonoSingletonPath.Instance.Log("Hello World!");
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
            Debug.LogFormat($"<color=#00ffffff>Class2MonoSingleton {GetHashCode()} {content}</color>");
        }
    }

    [MonoSingletonPath("TFrame/MonoSingeltonPath")]
    class Class2MonoSingletonPath : MonoSingleton<Class2MonoSingletonPath>
    {
        public void Log(string content)
        {
            Debug.LogFormat($"<color=#00ffffff>Class2MonoSingletonPath {GetHashCode()} {content}</color>");
        }
    }
}