using UnityEngine;

namespace TFrame
{
    public class TypeEventSystemExample : MonoBehaviour
    {
        private void Start()
        {
            //注册普通类型事件
            TypeEventSystem.Register<Data>(ReceiveClassData);
            //注册自动回收类型事件
            TypeEventSystem.Register<PoolableData>(ReceivePoolableData);
            Debug.LogFormat($"<color=#00ffffff>按“S”键发送事件，按“U”键注销事件</color>");
        }

        void Update()
        {
            //发送事件
            if (Input.GetKeyDown(KeyCode.S))
            {
                TypeEventSystem.Send(new Data { Name = "普通类型事件" });
                TypeEventSystem.Send<PoolableData>();
            }

            //注销事件
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.LogFormat($"<color=#00ffffff>注销事件</color>");
                TypeEventSystem.UnRegister<Data>(ReceiveClassData);
                TypeEventSystem.UnRegister<PoolableData>(ReceivePoolableData);
            }
        }

        private void ReceiveClassData(Data param)
        {
            Debug.LogFormat($"<color=#00ffffff>收到发送的事件，ClassData.Name:{param.Name}</color>");
        }

        private void ReceivePoolableData(PoolableData param)
        {
            Debug.LogFormat($"<color=#00ffffff>收到发送的事件，PoolableData 自动回收类</color>");
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
            Debug.LogFormat($"<color=#00ffffff>调用回收接口</color>");
        }
    }
}