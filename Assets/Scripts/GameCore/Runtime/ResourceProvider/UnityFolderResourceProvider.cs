using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class UnityFolderResourceProvider : IResourceProvider
    {
        public T GetResource<T>(string path) where T : UnityEngine.Object
        {
            var resource = Resources.Load<T>(path);

            if(resource == null)
            {
                Log.Error($"Resource null for path: \"{path}\"");
            }

            return resource;
        }
    }
}