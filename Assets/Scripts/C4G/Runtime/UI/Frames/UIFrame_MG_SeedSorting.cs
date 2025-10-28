using Cinemachine;
using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_SeedSorting : UIFrameMinigame
    {
        public const int STATE_STARTED = 5;
        public const int STATE_FINISHING = 10;
        public const int STATE_FINISHED = 15;

        [Header("Seed Sorting")]
        [SerializeField]
        private DialogPartDefinition _dialogStart;

        [SerializeField]
        private DialogPartDefinition _dialogEnd;

        [SerializeField]
        private GameObject _legendPanelObject;

        [SerializeField]
        private GameObject _holdingSeedImageObject;

        [SerializeField]
        private GameObject _fingerHintObject;

        [Header("Seed Sorting Icons")]
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

        [SerializeField]
        private Sprite _dirtIcon;

        [SerializeField]
        private Color _dirtColor;


        private GameObject _dialogCamGroupObject;

        private SeedSortingTag[] _possibleSeeds = new SeedSortingTag[]
        {
            SeedSortingTag.Grass,
            SeedSortingTag.Millet,
            SeedSortingTag.Sorghum,
            SeedSortingTag.Dirt
        };

        private Dictionary<SeedSortingTag, int> _correctSorts = new Dictionary<SeedSortingTag, int>()
        {
            { SeedSortingTag.Grass, 0 },
            { SeedSortingTag.Millet, 0 },
            { SeedSortingTag.Sorghum, 0 },
            { SeedSortingTag.Dirt, 0 }
        };

        private int _correctOfEachRequired = 4;

        private SeedSortingTag _holdingSeedType = SeedSortingTag.None;

        private SeedSortingBin _pileBin = null;


        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();

            _objectsToHideInDialog.Add(_legendPanelObject);
            _objectsToHideInDialog.Add(_fingerHintObject);

            var levelLayoutObject = GameObject.Find("LevelLayout_MG_SeedSorting");

            var mainVirtualCam = levelLayoutObject.transform.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            _viewManager.SetCameraAsPriority(mainVirtualCam);

            _dialogCamGroupObject = levelLayoutObject.transform.Find("DialogCameraGroup").gameObject;

            _interactionManager.PointerDownRaycastHit += OnPointerDown;
            _interactionManager.PointerClickRaycastHit += OnPointerClick;

            _holdingSeedImageObject.SetActive(false);

            // Do start dialog first, wait for other panels to load
            _timeManager.DoAfterShortDelay(() => { UiManager.PushUIDialog(_dialogStart, _dialogCamGroupObject); });
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
                    UpdateSorting();
                    break;
                case STATE_FINISHING:
                    // In ending dialog
                    break;
                case STATE_FINISHED:
                    // Done the minigame
                    OnMinigameComplete();
                    break;
            }

            if(currentMingameState != STATE_STARTED)
            {
                _fingerHintObject.SetActive(false);
            }
        }

        private void UpdateSorting()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), _inputManager.CurrentPointerPosition, null, out var anchoredPos);

            _holdingSeedImageObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos;

            if(_correctSorts[SeedSortingTag.Grass] >= _correctOfEachRequired &&
                _correctSorts[SeedSortingTag.Millet] >= _correctOfEachRequired &&
                _correctSorts[SeedSortingTag.Sorghum] >= _correctOfEachRequired)
            {
                SetMinigameState(STATE_FINISHING);
                UiManager.PushUIDialog(_dialogEnd, _dialogCamGroupObject);
            }
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
                case SeedSortingTag.Dirt:
                    holdingImage.sprite = _dirtIcon;
                    holdingImage.color = _dirtColor;
                    break;
            }
        }

        private void SelectSeedFromPile(SeedSortingBin seedPile)
        {
            _holdingSeedType = _possibleSeeds[UnityEngine.Random.Range(0, _possibleSeeds.Length)];

            _holdingSeedImageObject.SetActive(true);
            SetHoldingSeed(_holdingSeedType);

            seedPile.OnTakeSeed();
        }

        private void UpdatePileFullness()
        {
            if(_pileBin == null)
            {
                return;
            }

            var maxFull = _correctOfEachRequired * 3f;

            var fullness = (maxFull -
                Mathf.Min(_correctSorts[SeedSortingTag.Grass], _correctOfEachRequired) -
                Mathf.Min(_correctSorts[SeedSortingTag.Millet], _correctOfEachRequired) -
                Mathf.Min(_correctSorts[SeedSortingTag.Sorghum], _correctOfEachRequired)) / maxFull;

            _pileBin.SetFullness(fullness);

            Log.Debug($"Pile fullness: {fullness}");
        }

        private void DropSeed(SeedSortingBin seedSortingBin)
        {
            _holdingSeedImageObject.SetActive(false);

            if (seedSortingBin == null || seedSortingBin.SeedSortingTag == SeedSortingTag.Pile)
            {
                _holdingSeedType = SeedSortingTag.None;
                return;
            }

            if(seedSortingBin.SeedSortingTag == _holdingSeedType)
            {
                _correctSorts[_holdingSeedType]++;

                seedSortingBin.OnCorrectSeed();

                if (_holdingSeedType == SeedSortingTag.Grass ||
                    _holdingSeedType == SeedSortingTag.Millet ||
                    _holdingSeedType == SeedSortingTag.Sorghum)
                {
                    seedSortingBin.SetFullness(Mathf.Min(1f, (float)_correctSorts[_holdingSeedType] / _correctOfEachRequired));
                    UpdatePileFullness();
                }
            }
            else
            {
                seedSortingBin.OnIncorrectSeed();
            }

            _holdingSeedType = SeedSortingTag.None;
        }

        private void OnPointerDown(RaycastHit hitInfo)
        {
            // Check if pointer down happens on the seed pile
            var seedSortingBin = hitInfo.collider.GetComponentInParent<SeedSortingBin>();

            if(seedSortingBin != null && seedSortingBin.SeedSortingTag == SeedSortingTag.Pile)
            {
                _fingerHintObject.SetActive(false);

                _pileBin = seedSortingBin;
                SelectSeedFromPile(seedSortingBin);
            }
        }

        private void OnPointerClick(RaycastHit hitInfo)
        {
            // TODO: What if let go over ui (aka not catchable)

            var seedSortingBin = hitInfo.collider.GetComponentInParent<SeedSortingBin>();

            DropSeed(seedSortingBin);
        }
    }
}