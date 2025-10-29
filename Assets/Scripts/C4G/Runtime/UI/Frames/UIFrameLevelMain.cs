using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameLevelMain : AUIFrame
    {
        [Header("Components")]
        [SerializeField]
        private GameObject _hideInDialogPanel;

        [SerializeField]
        private Button _menuButton;

        [SerializeField]
        private Button _schoolButton;

        [SerializeField]
        private Button _characterSelectButton;

        [SerializeField]
        private Button _inventoryButton;

        [SerializeField]
        private GameObject _questPanel;

        [SerializeField]
        private TextMeshProUGUI _testQuestText;

        [SerializeField]
        private GameObject _inventoryItemParentPanel;

        [Header("Data")]
        [SerializeField]
        private GameSceneDefinition _schoolHubScene;

        [SerializeField]
        private GameSceneDefinition _startScene;

        [SerializeField]
        private GameVariableDefinition _minigameReturnToHub;

        [SerializeField]
        private UIFrameDefinition _inventoryFrameDefinition;

        [SerializeField]
        private GameObject _inventoryItemIconPrefab;

        private QuestManager _questManager;

        private PlayerInformationManager _playerInformationManager;
        private InteractionManager _interactionManager;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _menuButton.onClick.AddListener(OnMenuButonClicked);
            _schoolButton.onClick.AddListener(OnSchoolButtonClicked);
            _characterSelectButton.onClick.AddListener(OnCharacterSelectButtonClicked);
            _inventoryButton.onClick.AddListener(OnInventoryButtonClicked);

            UiManager.DialogStarted += OnDialogStarted;
            UiManager.DialogFinished += OnDialogFinished;

            _playerInformationManager = World.GetService<PlayerInformationManager>();
            _playerInformationManager.InventoryUpdated += OnInventoryUpdated;

            if(_playerInformationManager.CurrentPlayerInventory != null)
            {
                OnInventoryUpdated(_playerInformationManager.CurrentPlayerInventory);
            }

            _interactionManager = World.GetService<InteractionManager>();
            _interactionManager.PointerClickRaycastHit += OnPointerRaycastHit;

            World.GetService<GameVariableManager>().SetBool(_minigameReturnToHub.VariableId, false);
        }

        private void OnMenuButonClicked()
        {
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(_startScene);
        }

        private void OnSchoolButtonClicked()
        {
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(_schoolHubScene);
        }

        private void OnCharacterSelectButtonClicked()
        {
            UiManager.PushUICharacterSelect();
        }

        private void OnInventoryButtonClicked()
        {
            World.GetService<PlayerInformationManager>().CurrentPlayer.Navigator.StopNavigation();
            UiManager.PushUI(_inventoryFrameDefinition);
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            UiManager.DialogStarted -= OnDialogStarted;
            UiManager.DialogFinished -= OnDialogFinished;

            _interactionManager.PointerClickRaycastHit -= OnPointerRaycastHit;
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }

        private void Update()
        {
            if (_questManager == null) 
            {
                _questPanel.SetActive(false);

                _questManager = World.GetService<QuestManager>();

                if(_questManager != null)
                {
                    _questManager.QuestStepStarted += OnStepStarted;
                    _questManager.QuestComplete += OnQuestComplete;

                    if (_questManager.CurrentStep != null)
                    {
                        _questPanel.SetActive(true);
                        SetQuestText(_questManager.CurrentStep.Description);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            Log.Debug("Destroying UI Frame Level Main");

            if (_questManager != null) 
            {
                _questManager.QuestStepStarted -= OnStepStarted;
                _questManager.QuestComplete -= OnQuestComplete;
            }

            _playerInformationManager.InventoryUpdated -= OnInventoryUpdated;
        }

        private void OnStepStarted(IRequirementStep step)
        {
            _questPanel.SetActive(true);
            SetQuestText(step.Description);
        }

        private void SetQuestText(string description)
        {
            _testQuestText.text = $"{description}";
        }

        private void OnQuestComplete()
        {
            Debug.Log("Finished quest!");
            _questPanel.SetActive(false);

            _schoolButton.gameObject.AddComponent<UIWiggle>();
        }

        private void OnDialogStarted()
        {
            _hideInDialogPanel.SetActive(false);
        }

        private void OnDialogFinished()
        {
            _hideInDialogPanel.SetActive(true);
        }

        private void OnPointerRaycastHit(RaycastHit hitInfo)
        {
            var player = _playerInformationManager.CurrentPlayer;

            if(player == null)
            {
                return;
            }

            var interactionLimit = 22f;

            var hitLayer = hitInfo.collider.gameObject.layer;

            if (hitLayer == LayerMask.NameToLayer("Floor"))
            {
                player.Navigator.SetTargetDestination(hitInfo.point);
            }
            else if (hitLayer == LayerMask.NameToLayer("Player"))
            {
                player.Navigator.StopNavigation();
            }
            else
            {
                Log.Debug($"Unrecognized collider hit layer: {LayerMask.LayerToName(hitLayer)}");

                // If far enough away, try to move the player there
                if (hitInfo.distance >= interactionLimit)
                {
                    player.Navigator.SetTargetDestination(hitInfo.point);
                }
            }

            // Check for interactable methods
            var currentInteractableMethods = hitInfo.transform.GetComponentsInChildren<IInteractableMethod>().ToList();

            if (currentInteractableMethods.Count > 0 && hitInfo.distance < interactionLimit)
            {
                player.Navigator.StopNavigation();
                UiManager.PushUIInteractionChoice(currentInteractableMethods, hitInfo.transform.position);
            }
        }

        private void OnInventoryUpdated(Inventory inventory)
        {
            // huh????
            // maybe issues from chaining events to other events
            if(_inventoryItemParentPanel == null)
            {
                Log.Warning("Attempting to update inventory with null parent panel.");
                return;
            }

            foreach(Transform child in _inventoryItemParentPanel.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(var item in inventory.AllItems)
            {
                if(item.Amount == 0)
                {
                    continue;
                }

                var itemObj = Instantiate(_inventoryItemIconPrefab, _inventoryItemParentPanel.transform);

                itemObj.transform.Find("IconImage").GetComponent<Image>().sprite = item.Definition.UiIcon;
                var countingCircleTransform = itemObj.transform.Find("CounterCircle");

                if(item.Amount == 1)
                {
                    countingCircleTransform.gameObject.SetActive(false);
                }
                else
                {
                    countingCircleTransform.GetComponentInChildren<TextMeshProUGUI>().text = item.Amount.ToString();
                }
            }
        }
    }
}