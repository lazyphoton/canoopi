using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
    public class DummyLaunchParameters : ILaunchParameters
    {
        public bool TryGetValue(string key, out string value)
        {
            value = null;
            return false;
        }
    }
}