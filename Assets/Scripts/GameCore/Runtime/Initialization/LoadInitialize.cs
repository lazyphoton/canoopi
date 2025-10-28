using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public static class LoadInitialize
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoadMethod()
        {
            Log.Initialize();
            World.Initialize();
        }
    }
}