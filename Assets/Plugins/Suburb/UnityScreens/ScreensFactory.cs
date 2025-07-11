﻿using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Suburb.Screens
{
    public class ScreensFactory : IFactory<Type, BaseScreen>, IDisposable
    {
        private readonly DiContainer container;
        private readonly string screensRootPath;

        private readonly Transform uiRoot;

        public ScreensFactory(DiContainer container, string screensRootPath)
        {
            this.container = container;
            this.screensRootPath = screensRootPath;

            var uiRootGameObject = new GameObject("UIRoot");
            GameObject.DontDestroyOnLoad(uiRootGameObject);
            uiRoot = uiRootGameObject.transform;
        }

        public BaseScreen Create(Type screenType)
        {
            string resourcePath = $"{screensRootPath}/{screenType.Name}";
            var prefab = Resources.Load(resourcePath);

            return (BaseScreen)container.InstantiatePrefabForComponent(
                screenType, prefab, uiRoot, Array.Empty<object>());
        }

        public void Dispose()
        {
            Object.Destroy(uiRoot.gameObject);
        }
    }
}
