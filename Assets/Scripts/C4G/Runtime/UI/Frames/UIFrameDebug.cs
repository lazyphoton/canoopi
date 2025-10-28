using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameDebug : AUIFrame
    {
        [SerializeField]
        private Button _closeButton;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            PopSelf();
        }

        public void LoadScene(string sceneName)
        {
            World.GetService<ISceneManager>().LoadSceneSingleAsync(sceneName);
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {

        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}
