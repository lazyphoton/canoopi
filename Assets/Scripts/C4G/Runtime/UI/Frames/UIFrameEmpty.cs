using System.Collections.Generic;
using UnityEngine;
using GameCore;

namespace c4g
{
    /// <summary>
    /// An empty UI frame that can be used as a basis for
    /// a scene's UI initialization if an initial frame is not needed.
    /// </summary>
    public class UIFrameEmpty : AUIFrame
    {
        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}