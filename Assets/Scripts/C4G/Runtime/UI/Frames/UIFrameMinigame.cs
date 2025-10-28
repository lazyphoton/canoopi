using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public abstract class UIFrameMinigame : AUIFrame
    {
        [Header("Base Components")]
        [SerializeField]
        private Button _schoolButton;

        [SerializeField]
        private Button _completeButton;

        [Header("Base Data")]
        [SerializeField]
        private GameSceneDefinition _schoolHubScene;

        [SerializeField]
        private GameSceneDefinition _returnScene;

        [SerializeField]
        private GameVariableDefinition _minigameReturnToHub;

        [SerializeField]
        private GameVariableDefinition _minigameProgressState;

        [SerializeField]
        private GameVariableChange[] _gameVariablesOnComplete = new GameVariableChange[0];


        protected GameVariableManager _gameVariableManager;
        protected TimeManager _timeManager;
        protected InteractionManager _interactionManager;
        protected ViewManager _viewManager;
        protected InputManager _inputManager;
        protected PlayerInformationManager _playerInformationManager;

        protected List<GameObject> _objectsToHideInDialog;

        protected void BaseMinigameSetup()
        {
            _timeManager = World.GetService<TimeManager>();
            _gameVariableManager = World.GetService<GameVariableManager>();
            _interactionManager = World.GetService<InteractionManager>();
            _viewManager = World.GetService<ViewManager>();
            _inputManager = World.GetService<InputManager>();
            _playerInformationManager = World.GetService<PlayerInformationManager>();

            SetMinigameState(0);

            _objectsToHideInDialog = new List<GameObject>();

            UiManager.DialogStarted += OnDialogStarted;
            UiManager.DialogFinished += OnDialogFinished;

            _schoolButton.onClick.AddListener(OnSchoolButtonClicked);
            _completeButton.onClick.AddListener(OnCompleteClicked);
        }

        protected void BaseMinigameTeardown()
        {
            UiManager.DialogStarted -= OnDialogStarted;
            UiManager.DialogFinished -= OnDialogFinished;
        }

        protected void SetMinigameState(int value)
        {
            _gameVariableManager.SetInt(_minigameProgressState.VariableId, value);
        }

        protected int GetMinigameState()
        {
            if(_gameVariableManager.TryGetInt(_minigameProgressState.VariableId, out var value))
            {
                return value;
            }

            return -1;
        }

        private void OnSchoolButtonClicked()
        {
            LoadScene(_schoolHubScene);
        }

        private void OnCompleteClicked()
        {
            OnMinigameComplete();
        }

        protected void OnMinigameComplete()
        {
            // Maybe messy?
            // This is to "freeze" any minigame logic after the minigajme has been stopped
            SetMinigameState(-1);

            // Only apply the finishing variables and return to the specified return scene
            // if it's not indicated to return to the school hub.
            if(_gameVariableManager.TryGetBool(_minigameReturnToHub.VariableId, out var returnToHub) && returnToHub)
            {
                LoadScene(_schoolHubScene);
            }
            else
            {
                foreach (var change in _gameVariablesOnComplete)
                {
                    change.ApplyChange();
                }

                LoadScene(_returnScene);
            }
        }

        private void LoadScene(GameSceneDefinition scene)
        {
            _gameVariableManager.SetBool(_minigameReturnToHub.VariableId, false);
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(scene);
        }

        private void OnDialogStarted()
        {
            foreach(var obj in _objectsToHideInDialog)
            {
                obj.SetActive(false);
            }
        }

        private void OnDialogFinished()
        {
            foreach (var obj in _objectsToHideInDialog)
            {
                obj.SetActive(true);
            }
        }
    }
}