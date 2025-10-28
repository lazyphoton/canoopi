using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public interface ISceneManager
    {
        public void LoadSceneAsync(string name, LoadSceneMode mode);
    }
}