using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public static class ISceneManagerExtensions
    {
        public static void LoadSceneAdditiveAsync(this ISceneManager sceneManager, string name)
        {
            sceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        public static void LoadSceneSingleAsync(this ISceneManager sceneManager, string name)
        {
            sceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        }
    }
}