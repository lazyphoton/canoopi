using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public static class IVisualizableExtensions
    {
        public static void SetVisualWithInstantiation(this IVisualizable visualizable, GameObject prefab)
        {
            visualizable.SetVisual(GameObject.Instantiate(prefab));
        }
    }
}