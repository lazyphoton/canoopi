using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class AUIFrame : MonoBehaviour
    {
        public UIManager UiManager { get; private set; }

        public void OnPush(UIManager uiManager, Dictionary<string, object> injectedInfo)
        {
            UiManager = uiManager;
            OnPush(injectedInfo);
        }

        public abstract void OnPush(Dictionary<string, object> injectedInfo);

        public abstract void OnPop(Dictionary<string, object> injectedInfo);

        public abstract void OnFallbackFocus(Dictionary<string, object> injectedInfo);

        // Technically this could run into issues
        // if called indirectly from a different UI frame
        // Maybe worth revisiting if necessary
        protected void PopSelf()
        {
            PopSelf(new Dictionary<string, object>());
        }

        protected void PopSelf(Dictionary<string, object> injectedInfo)
        {
            UiManager.PopUI(injectedInfo);
        }
    }
}