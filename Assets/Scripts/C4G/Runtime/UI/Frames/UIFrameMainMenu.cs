using GameCore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameMainMenu : AUIFrame
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private TextMeshProUGUI _versionText;

        [SerializeField]
        private GameSceneDefinition _schoolHubScene;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _versionText.text = $"version: {Application.version}";
        }

        private void OnPlayButtonClicked()
        {
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(_schoolHubScene);
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}