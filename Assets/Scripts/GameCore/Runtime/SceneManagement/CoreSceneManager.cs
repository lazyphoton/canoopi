using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public class CoreSceneManager : ISceneManager
    {
        public CoreSceneManager() {}

        public void LoadSceneAsync(string name, LoadSceneMode mode)
        {
            if(mode == LoadSceneMode.Single)
            {
                World.Services.RemoveNonPersistentServices();
            }

            SceneManager.LoadSceneAsync(name, mode);
        }
    }
}