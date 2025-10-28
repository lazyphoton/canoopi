using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public enum DialogHeadshotPosition
    {
        Left,
        Right
    }

    public class UIFrameDialog : AUIFrame
    {
        public const string UIKEY_StartingDialogPart = "UIFrameDialogStartingDialogPart";
        public const string UIKEY_DialogCameraGroupObject = "UIFrameDialogCameraGroupObject";

        [SerializeField]
        private TextMeshProUGUI _dialogText;

        [SerializeField]
        private GameObject _headshotPanelLeft;

        [SerializeField]
        private Image _headshotImageLeft;

        [SerializeField]
        private GameObject _headshotPanelRight;

        [SerializeField]
        private Image _headshotImageRight;

        [SerializeField]
        private Button _fullButton;

        [Header("Dialog Image")]
        [SerializeField]
        private GameObject _dialogImageBackground;

        [SerializeField]
        private Image _dialogImage;

        [SerializeField]
        private GameVariableDefinition _dialogImageGameVariable;

        [Header("Dialog Camera")]
        [SerializeField]
        private GameVariableDefinition _dialogCameraGameVariable;

        private DialogPartDefinition _currentDialogPart;

        private GameVariableManager _gameVariableManager;
        private ViewManager _viewManager;

        private DialogCameraGroup _dialogCameraGroup;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _fullButton.onClick.AddListener(OnFullButtonClicked);

            _gameVariableManager = World.GetService<GameVariableManager>();
            _gameVariableManager.GameVariableChanged += OnGameVariableChanged;

            _viewManager = World.GetService<ViewManager>();

            _currentDialogPart = (DialogPartDefinition)injectedInfo[UIKEY_StartingDialogPart];

            UiManager.SetDialogStarted();

            if (_currentDialogPart == null)
            {
                Log.Error("Null dialog part on dialog panel push");
                PopSelf();
                return;
            }

            var camGroupObj = (GameObject)injectedInfo[UIKEY_DialogCameraGroupObject];

            if(camGroupObj != null && camGroupObj.TryGetComponent<DialogCameraGroup>(out _dialogCameraGroup))
            {
                _viewManager.PushCamera(_dialogCameraGroup.VirtualCamera);
                _dialogCameraGroup.SetDefaultCameraTransform();
            }

            Log.Debug($"Starting dialog with part: {_currentDialogPart.name}");

            ShowCurrentDialogPart();
        }

        private void OnFullButtonClicked()
        {
            GoToNextStep();
        }

        private void GoToNextStep() 
        {
            // Apply variable changes at end of current step
            ApplyGameVariableChanges(_currentDialogPart.GameVariablesToSetOnPartEnd);

            DialogPartDefinition nextDialogPart = null;

            foreach (var conditionalNextPart in _currentDialogPart.ConditionalNextDialogParts) 
            {
                if (conditionalNextPart.GameCondition.IsConditionMet()) 
                {
                    nextDialogPart = conditionalNextPart.NextDialogPart;
                    break;
                }
            }

            if(nextDialogPart == null)
            {
                nextDialogPart = _currentDialogPart.DefaultNextDialogPart;
            }

            if(nextDialogPart == null)
            {
                PopSelf();
                return;
            }

            _currentDialogPart = nextDialogPart;
            ShowCurrentDialogPart();
        }

        private void ShowCurrentDialogPart()
        {
            _dialogImageBackground.SetActive(false);

            // Apply variable changes at start of current step
            ApplyGameVariableChanges(_currentDialogPart.GameVariablesToSetOnPartStart);

            if (string.IsNullOrWhiteSpace(_currentDialogPart.DialogText))
            {
                GoToNextStep();
                return;
            }

            _dialogText.text = _currentDialogPart.DialogText;

            if (_currentDialogPart.HeadshotPosition == DialogHeadshotPosition.Left)
            {
                _headshotPanelLeft.SetActive(true);
                _headshotPanelRight.SetActive(false);

                _headshotImageLeft.sprite = _currentDialogPart.CharacterDefinition.CharacterHeadshot;
            }
            else if (_currentDialogPart.HeadshotPosition == DialogHeadshotPosition.Right)
            {
                _headshotPanelRight.SetActive(true);
                _headshotPanelLeft.SetActive(false);

                _headshotImageRight.sprite = _currentDialogPart.CharacterDefinition.CharacterHeadshot;
            }
            else
            {
                Log.Error($"Unrecognized dialog headshot position: {_currentDialogPart.HeadshotPosition}");
            }
        }

        private void ApplyGameVariableChanges(List<GameVariableChange> changes)
        {
            foreach (var change in changes)
            {
                change.ApplyChange();
            }
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            Log.Debug("Popping dialog frame.");
            _gameVariableManager.GameVariableChanged -= OnGameVariableChanged;

            UiManager.SetDialogFinished();

            if (_dialogCameraGroup != null)
            {
                _viewManager.PopCamera();
            }
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }

        private void OnGameVariableChanged(GameVariableDefinition gameVariableDefintion)
        {
            if(gameVariableDefintion == _dialogImageGameVariable)
            {
                UpdateDialogImage();
            }
            else if(gameVariableDefintion == _dialogCameraGameVariable)
            {
                UpdateDialogCamera();
            }
        }

        private void UpdateDialogImage()
        {
            var showImage = false;

            if (_gameVariableManager.TryGetString(_dialogImageGameVariable.VariableId, out var value) &&
                !string.IsNullOrWhiteSpace(value))
            {
                var image = World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", value));

                if (image != null)
                {
                    _dialogImage.sprite = image;
                    showImage = true;
                }
            }

            _dialogImageBackground.SetActive(showImage);
        }

        private void UpdateDialogCamera()
        {
            if(_gameVariableManager.TryGetInt(_dialogCameraGameVariable.VariableId, out var value))
            {
                _dialogCameraGroup.SetCameraTransformIndex(value);
            }
        }
    }
}