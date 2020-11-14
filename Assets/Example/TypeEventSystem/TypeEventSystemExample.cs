using UnityEngine;

namespace TFrame
{
    public class TypeEventSystemExample : MonoBehaviour
    {
        private void Start()
        {
            //注册事件
            TypeEventSystem.Register<Data>(ReceiveClassData);
            TypeEventSystem.Register<PoolableData>((_)=> { Debug.LogError(666); });
        }

        void Update()
        {
            //发送事件
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.LogFormat($"<color=#00ffffff>发送事件 ClassData</color>");
                TypeEventSystem.Send(new Data { Name = "没有感情的工具类" });
                TypeEventSystem.Send<PoolableData>();
            }

            //注销事件
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.LogFormat($"<color=#00ffffff>注销事件 ClassData</color>");
                TypeEventSystem.UnRegister<Data>(ReceiveClassData);
            }
        }

        private void ReceiveClassData(Data param)
        {
            Debug.LogFormat($"<color=#00ffffff>收到发送的事件，ClassData.Name:{param.Name}</color>");
        }
    }

    class Data
    {
        public string Name;
    }

    class PoolableData : IPoolable
    {
        bool IPoolable.IsRecycled { get; set; }

        void IPoolable.OnRecycled()
        {
            Debug.LogFormat($"<color=#00ffffff>可被回收的数据类</color>");
        }
    }
}