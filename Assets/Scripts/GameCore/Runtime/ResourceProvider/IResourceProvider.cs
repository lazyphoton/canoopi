using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public interface IResourceProvider
    {
        public T GetResource<T>(string path) where T : UnityEngine.Object;
    }
}