using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

namespace c4g
{
    [InitializeService]
    public class InitializeC4G
    {
        public InitializeC4G()
        {
            // This only ends up initializing it once before initial scene load because of the attribute
            // For scene-based initializations, use the scene initializer instead

            //World.Services.GetService<ISceneManager>().LoadSceneAdditiveAsync("CommonSystems");
        }
    }
}