using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameCore
{
    public static class World
    {
        public static ServiceManager Services { get; private set; }

        private static GameObject _globalComponentsObject;

        public static void Initialize()
        {
            if (_globalComponentsObject != null)
            {
                Log.Warning("Re-initializing world and global components...");
                GameObject.Destroy(_globalComponentsObject);
            }

            _globalComponentsObject = new GameObject("GlobalComponents");
            UnityEngine.Object.DontDestroyOnLoad(_globalComponentsObject);

            Services = new ServiceManager(_globalComponentsObject);

#if UNITY_WEBGL
            Services.SetService(typeof(ILaunchParameters), new WebglLaunchParameters(), true);
#else
            Services.SetService(typeof(ILaunchParameters), new DummyLaunchParameters(), true);
#endif

            Services.SetService(typeof(ISceneManager), new CoreSceneManager(), true);
            Services.CreateComponentService(typeof(Awaiter), typeof(Awaiter), true);
            Services.CreateComponentService(typeof(TimeManager), typeof(TimeManager), true);

            var serviceInitializationClasses = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.GetCustomAttribute<InitializeServiceAttribute>() != null);

            foreach(var type in serviceInitializationClasses)
            {
                Activator.CreateInstance(type);
            }
        }

        public static T GetService<T>() where T : class
        {
            return Services.GetService<T>();
        }
    }
}