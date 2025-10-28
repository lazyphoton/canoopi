using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_BundDigging : UIFrameMinigame
    {
        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameTeardown();
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}