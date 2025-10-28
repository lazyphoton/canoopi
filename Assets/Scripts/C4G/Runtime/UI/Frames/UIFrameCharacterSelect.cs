using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameCharacterSelect : AUIFrame
    {
        [SerializeField]
        private Button _selectButton;

        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private Button _previousButton;

        [SerializeField]
        private VisualData _visualData;

        [SerializeField]
        private GameObject _characterSelectionVisPrefab;

        private int _selectedVisualIndex;

        private PlayerInformationManager _playerInformationManager;
        private GameObject _characterSelectionVisObj;
        private CharacterSelectionVisual _characterSelectionVisual;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _playerInformationManager = World.GetService<PlayerInformationManager>();

            // A bit of a hack to save the currently selkected character visual
            // and prevent the popup from happening all the time.
            if (_playerInformationManager.HasPlayerChosenVisual)
            {
                Log.Debug("Skipping character selection and using saved visual index from player information manager.");
                SetPlayerVisual(_playerInformationManager.CurrentPlayerVisualIndex);
                PopSelf();
                return;
            }

            _selectButton.onClick.AddListener(OnSelectButtonClicked);
            _nextButton.onClick.AddListener(OnNextButtonClicked);
            _previousButton.onClick.AddListener(OnPreviousButtonClicked);

            _selectedVisualIndex = 0;

            _characterSelectionVisObj = Instantiate(_characterSelectionVisPrefab);
            _characterSelectionVisObj.transform.position = new Vector3(0f, -500f, 0f);

            _characterSelectionVisual = _characterSelectionVisObj.GetComponent<CharacterSelectionVisual>();
            _characterSelectionVisual.Initialize(_visualData.VisualPrefabs);
        }

        private void OnSelectButtonClicked()
        {
            SetPlayerVisual(_selectedVisualIndex);
            Destroy(_characterSelectionVisObj);

            PopSelf();
        }

        private void SetPlayerVisual(int index)
        {
            var selectedVisual = _visualData.VisualPrefabs[index];
            _playerInformationManager.CurrentPlayerVisualIndex = index;
            _playerInformationManager.CurrentPlayer.SetVisualWithInstantiation(selectedVisual);
        }

        private void OnNextButtonClicked()
        {
            UpdateVisualIndex(1);
        }

        private void OnPreviousButtonClicked()
        {
            UpdateVisualIndex(-1);
        }

        private void UpdateVisualIndex(int offset)
        {
            _selectedVisualIndex += offset;

            if (_selectedVisualIndex > _visualData.VisualPrefabs.Length - 1)
            {
                _selectedVisualIndex = 0;
            }
            
            if(_selectedVisualIndex < 0)
            {
                _selectedVisualIndex = _visualData.VisualPrefabs.Length - 1;
            }

            _selectButton.interactable = false;
            _nextButton.interactable = false;
            _previousButton.interactable = false;

            _characterSelectionVisual.MoveToIndex(_selectedVisualIndex, OnMoveFinished);
        }

        private void OnMoveFinished()
        {
            _selectButton.interactable = true;
            _nextButton.interactable = true;
            _previousButton.interactable = true;
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            Log.Debug("Popping character selectiuon frame.");
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}