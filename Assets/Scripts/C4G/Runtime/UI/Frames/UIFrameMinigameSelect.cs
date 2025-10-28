using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameMinigameSelect : AUIFrame
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private GameVariableDefinition _minigameReturnToHub;

        [Header("Levels")]
        [SerializeField]
        private Transform _levelButtonParent;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);

            foreach(var levelButton in _levelButtonParent.GetComponentsInChildren<LevelButton>())
            {
                levelButton.GetComponent<Button>().onClick.AddListener(() => { OnLevelButtonClicked(levelButton.Scene); });
            }
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

        private void OnLevelButtonClicked(GameSceneDefinition sceneDefinition)
        {
            World.GetService<GameVariableManager>().SetBool(_minigameReturnToHub.VariableId, true);
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(sceneDefinition);
        }
    }
}