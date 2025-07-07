using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TestTask
{
    public class LayoutsRoot : IInitializable, IDisposable
    {
        public Transform Root { get; private set; }
        
        public void Initialize()
        {
            Root = new GameObject("LayoutsRoot").transform;
            Object.DontDestroyOnLoad(Root.gameObject);
        }

        public void Dispose()
        {
            Object.Destroy(Root.gameObject);
        }
    }
}