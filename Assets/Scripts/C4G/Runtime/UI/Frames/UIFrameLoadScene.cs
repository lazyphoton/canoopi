using GameCore;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameLoadScene : AUIFrame
    {
        public const string UIKEY_LoadScene = "UIFrameLoadScene";

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _goButton;

        [SerializeField]
        private TextMeshProUGUI _infoText;

        private GameSceneDefinition _scene;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            if(injectedInfo.TryGetValue(UIKEY_LoadScene, out var scene))
            {
                _scene = (GameSceneDefinition)scene;

                _infoText.text = $"Load scene: \"{_scene.SceneName}\"?\n(placeholder to be used for loading different levels)";

                _closeButton.onClick.AddListener(OnCloseButtonClicked);
                _goButton.onClick.AddListener(OnGoButtonClicked);
            }
            else
            {
                Log.Error("No scene name provided for UIFrameLoadScene");
                PopSelf();
            }
        }

        private void OnCloseButtonClicked()
        {
            PopSelf();
        }

        private void OnGoButtonClicked()
        {
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(_scene);
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}