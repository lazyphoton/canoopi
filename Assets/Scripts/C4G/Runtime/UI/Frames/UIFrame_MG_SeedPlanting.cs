using Cinemachine;
using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_SeedPlanting : UIFrameMinigame
    {
        public const int STATE_STARTED = 5;
        public const int STATE_FINISHING = 10;
        public const int STATE_FINISHED = 15;

        [Header("Seed Planting")]
        [SerializeField]
        private DialogPartDefinition _dialogStart;

        [SerializeField]
        private DialogPartDefinition _dialogEnd;

        [SerializeField]
        private GameObject _holdingSeedImageObject;

        [Header("Seed Planting Icons")]
        [SerializeField]
        private Sprite _grassSeedIcon;

        [SerializeField]
        private Color _grassColor;

        [SerializeField]
        private Sprite _milletSeedIcon;

        [SerializeField]
        private Color _milletColor;

        [SerializeField]
        private Sprite _sorghumSeedIcon;

        [SerializeField]
        private Color _sorghumColor;


        //private GameObject _dialogCamGroupObject;

        private SeedSortingTag _holdingSeedType = SeedSortingTag.None;

        private int _requiredCorrectCount = 13;

        private int _currentCorrectCount = 0;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();

            var levelLayoutObject = GameObject.Find("LevelLayout_MG_SeedPlanting");

            var mainVirtualCam = levelLayoutObject.transform.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            _viewManager.SetCameraAsPriority(mainVirtualCam);

            //_dialogCamGroupObject = levelLayoutObject.transform.Find("DialogCameraGroup").gameObject;

            _interactionManager.PointerDownRaycastHit += OnPointerDown;
            _interactionManager.PointerClickRaycastHit += OnPointerClick;

            _holdingSeedImageObject.SetActive(false);

            // Do start dialog first, wait for other panels to load
            _timeManager.DoAfterShortDelay(() => { UiManager.PushUIDialog(_dialogStart, null); });
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            _interactionManager.PointerDownRaycastHit -= OnPointerDown;
            _interactionManager.PointerClickRaycastHit -= OnPointerClick;

            BaseMinigameTeardown();
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }

        private void Update()
        {
            var currentMingameState = GetMinigameState();

            switch (currentMingameState)
            {
                case 0:
                    // In starting dialog
                    break;
                case STATE_STARTED:
                    // Doing the game
                    UpdatePlanting();
                    break;
                case STATE_FINISHING:
                    // In ending dialog
                    break;
                case STATE_FINISHED:
                    // Done the minigame
                    OnMinigameComplete();
                    break;
            }
        }

        private void UpdatePlanting()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), _inputManager.CurrentPointerPosition, null, out var anchoredPos);

            _holdingSeedImageObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos;

            if (_currentCorrectCount >= _requiredCorrectCount)
            {
                SetMinigameState(STATE_FINISHING);
                UiManager.PushUIDialog(_dialogEnd, null);
            }
        }

        private void SelectSeedFromBin(SeedSortingBin seedSortingBin)
        {
            if(seedSortingBin == null)
            {
                return;
            }

            if(seedSortingBin.SeedSortingTag != SeedSortingTag.Grass &&
                seedSortingBin.SeedSortingTag != SeedSortingTag.Millet &&
                seedSortingBin.SeedSortingTag != SeedSortingTag.Sorghum)
            {
                return;
            }

            _holdingSeedType = seedSortingBin.SeedSortingTag;
            _holdingSeedImageObject.SetActive(true);
            SetHoldingSeed(_holdingSeedType);
            seedSortingBin.OnTakeSeed();
        }

        private void SetHoldingSeed(SeedSortingTag seedType)
        {
            var holdingImage = _holdingSeedImageObject.GetComponent<Image>();

            switch (seedType)
            {
                case SeedSortingTag.Grass:
                    holdingImage.sprite = _grassSeedIcon;
                    holdingImage.color = _grassColor;
                    break;
                case SeedSortingTag.Millet:
                    holdingImage.sprite = _milletSeedIcon;
                    holdingImage.color = _milletColor;
                    break;
                case SeedSortingTag.Sorghum:
                    holdingImage.sprite = _sorghumSeedIcon;
                    holdingImage.color = _sorghumColor;
                    break;
            }
        }

        private void DropSeed(SeedPlantingTarget seedPlantingTarget)
        {
            _holdingSeedImageObject.SetActive(false);

            if(seedPlantingTarget == null)
            {
                _holdingSeedType = SeedSortingTag.None;
                return;
            }

            if(seedPlantingTarget.SeedSortingTag == _holdingSeedType)
            {
                seedPlantingTarget.OnCorrectSeed();
                _currentCorrectCount++;
            }
            else
            {
                seedPlantingTarget.OnIncorrectSeed();
            }

            _holdingSeedType = SeedSortingTag.None;
        }

        private void OnPointerDown(RaycastHit hitInfo)
        {
            // Check if pointer down happens on a seed bin
            var seedSortingBin = hitInfo.collider.GetComponentInParent<SeedSortingBin>();

            SelectSeedFromBin(seedSortingBin);
        }

        private void OnPointerClick(RaycastHit hitInfo)
        {
            // TODO: What if let go over ui (aka not catchable)

            var seedPlantingtarget = hitInfo.collider.GetComponentInParent<SeedPlantingTarget>();

            DropSeed(seedPlantingtarget);
        }
    }
}