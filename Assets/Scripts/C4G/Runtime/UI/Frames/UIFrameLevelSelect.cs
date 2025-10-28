using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameLevelSelect : AUIFrame
    {
        [SerializeField]
        private Button _closeButton;

        [Header("Levels")]
        [SerializeField]
        private Button _levelOneButton;

        [SerializeField]
        private GameSceneDefinition _bundsScene;


        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _levelOneButton.onClick.AddListener(OnLevelOneButtonClicked);
        }
        
        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {
            
        }

        private void OnCloseButtonClicked()
        {
            PopSelf();
        }

        private void OnLevelOneButtonClicked() 
        {
            // TODO: Parametrization and state set/reset?

            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(_bundsScene);
        }
    }
}