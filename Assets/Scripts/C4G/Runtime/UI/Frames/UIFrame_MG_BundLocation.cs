using Cinemachine;
using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_BundLocation : UIFrameMinigame
    {
        public const int STATE_GROUND = 5;
        public const int STATE_SLOPE = 10;
        public const int STATE_POSITION = 15;
        public const int STATE_FINISHING = 20;
        public const int STATE_FINISHED = 25;

        [Header("Bund Location Dialogs")]
        [SerializeField]
        private DialogPartDefinition _dialogGround;

        [SerializeField]
        private DialogPartDefinition _dialogGroundRocky;

        [SerializeField]
        private DialogPartDefinition _dialogGroundSandy;

        [SerializeField]
        private DialogPartDefinition _dialogSlope;

        [SerializeField]
        private DialogPartDefinition _dialogSlopeSteep;

        [SerializeField]
        private DialogPartDefinition _dialogPosition;

        [SerializeField]
        private DialogPartDefinition _dialogEnd;

        [Header("Bund Position")]
        [SerializeField]
        private GameObject _posButtonParentObject;

        [SerializeField]
        private Button _posButtonUp;

        [SerializeField]
        private Button _posButtonRight;

        [SerializeField]
        private Button _posButtonDown;

        [SerializeField]
        private Button _posButtonLeft;

        [SerializeField]
        private Button _posButtonRotateRight;

        [SerializeField]
        private Button _posButtonRotateLeft;


        private bool _groundSelected = false;
        private bool _slopeSelected = false;

        private CinemachineVirtualCamera _cameraGround;
        private CinemachineVirtualCamera _cameraSlope;
        private CinemachineVirtualCamera _cameraPosition;

        private GameObject _dialogCamGroupObject;

        private GameObject _playerPositionBundObject;

        private int _playerPositionXMin = -5;
        private int _playerPositionXMax = 20;
        private int _playerPositionZMin = -16;
        private int _playerPositionZMax = 3;

        private Vector2Int[] _possibleCorrectPositions = new Vector2Int[]
        {
            new Vector2Int(9, -4),
            new Vector2Int(9, -12),
            new Vector2Int(3, -12)
        };

        private Vector2Int _currentPlayerPosition = new Vector2Int(14, -14);
        private int _currentPlayerRotation = 2;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();

            _interactionManager.PointerDownRaycastHit += OnPointerDown;
            _interactionManager.PointerClickRaycastHit += OnPointerClick;

            var levelLayoutObject = GameObject.Find("LevelLayout_MG_BundLocation");

            _cameraGround = levelLayoutObject.transform.Find("VirtualCameraGround").GetComponent<CinemachineVirtualCamera>();
            _cameraSlope = levelLayoutObject.transform.Find("VirtualCameraSlope").GetComponent<CinemachineVirtualCamera>();
            _cameraPosition = levelLayoutObject.transform.Find("VirtualCameraPosition").GetComponent<CinemachineVirtualCamera>();

            _viewManager.SetCameraAsPriority(_cameraGround);

            _dialogCamGroupObject = levelLayoutObject.transform.Find("DialogCameraGroup").gameObject;

            _playerPositionBundObject = levelLayoutObject.transform.Find("BundsLayout").Find("PlayerPositionBund").gameObject;

            _posButtonUp.onClick.AddListener(OnPosButtonUp);
            _posButtonRight.onClick.AddListener(OnPosButtonRight);
            _posButtonDown.onClick.AddListener(OnPosButtonDown);
            _posButtonLeft.onClick.AddListener(OnPosButtonLeft);
            _posButtonRotateRight.onClick.AddListener(OnPosRotateRight);
            _posButtonRotateLeft.onClick.AddListener(OnPosRotateLeft);

            _posButtonParentObject.SetActive(false);

            // Do start dialog first, wait for other panels to load
            _timeManager.DoAfterShortDelay(() => { UiManager.PushUIDialog(_dialogGround, _dialogCamGroupObject); });
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
                case STATE_GROUND:
                    // Selecting the ground
                    UpdateStateGround();
                    break;
                case STATE_SLOPE:
                    // Selecting the slope
                    UpdateStateSlope();
                    break;
                case STATE_POSITION:
                    // Selecting the position
                    UpdateStatePosition();
                    break;
                case STATE_FINISHING:
                    // In ending dialog
                    break;
                case STATE_FINISHED:
                    // Done the minigame
                    OnMinigameComplete();
                    break;
            }

            if(currentMingameState != STATE_POSITION)
            {
                _posButtonParentObject.SetActive(false);
            }
        }

        private void UpdateStateGround()
        {
            // Check if player has selected the correct ground type
            if (_groundSelected)
            {
                SetMinigameState(STATE_SLOPE);
                _viewManager.SetCameraAsPriority(_cameraSlope);
                UiManager.PushUIDialog(_dialogSlope, _dialogCamGroupObject);
            }
        }

        private void UpdateStateSlope()
        {
            // Check if player has selected the correct slope position
            if (_slopeSelected)
            {
                SetMinigameState(STATE_POSITION);
                _viewManager.SetCameraAsPriority(_cameraPosition);
                _objectsToHideInDialog.Add(_posButtonParentObject);

                UiManager.PushUIDialog(_dialogPosition, _dialogCamGroupObject);
            }
        }

        private void UpdateStatePosition() 
        {
            // Check if player has lined up the bund correctly
            if (IsPlayerBundInCorrectPosition())
            {
                SetMinigameState(STATE_FINISHING);
                UiManager.PushUIDialog(_dialogEnd, _dialogCamGroupObject);
            }
        }

        private bool IsPlayerBundInCorrectPosition()
        {
            if (_currentPlayerRotation != 0)
            {
                return false;
            }

            foreach (var potentialPosition in _possibleCorrectPositions)
            {
                if(potentialPosition == _currentPlayerPosition)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnBundOptionClicked(BundLocationTag locationTag)
        {
            switch (locationTag) 
            {
                case BundLocationTag.GroundRocky:
                    UiManager.PushUIDialog(_dialogGroundRocky, _dialogCamGroupObject);
                    break;
                case BundLocationTag.GroundSandy:
                    UiManager.PushUIDialog(_dialogGroundSandy, _dialogCamGroupObject);
                    break;
                case BundLocationTag.GroundSoil:
                    _groundSelected = true;
                    break;
                case BundLocationTag.SlopeSteep:
                    UiManager.PushUIDialog(_dialogSlopeSteep, _dialogCamGroupObject);
                    break;
                case BundLocationTag.SlopeShallow:
                    _slopeSelected = true;
                    break;
            }
        }

        private void OnPointerDown(RaycastHit hitInfo)
        {
            
        }

        private void OnPointerClick(RaycastHit hitInfo)
        {
            var bundLocationOption = hitInfo.collider.GetComponentInParent<BundLocationOption>();

            if(bundLocationOption != null)
            {
                OnBundOptionClicked(bundLocationOption.BundLocationTag);
            }
        }

        private void OnPosButtonUp()
        {
            UpdatePosition(0, 1, 0);
        }

        private void OnPosButtonRight()
        {
            UpdatePosition(1, 0, 0);
        }

        private void OnPosButtonDown()
        {
            UpdatePosition(0, -1, 0);
        }

        private void OnPosButtonLeft()
        {
            UpdatePosition(-1, 0, 0);
        }

        private void OnPosRotateRight()
        {
            UpdatePosition(0, 0, 1);
        }

        private void OnPosRotateLeft()
        {
            UpdatePosition(0, 0, -1);
        }

        private void UpdatePosition(int horizontalChange, int verticalChange, int rotationChange)
        {
            _currentPlayerPosition.x += horizontalChange;
            _currentPlayerPosition.y += verticalChange;
            _currentPlayerRotation += rotationChange;

            if(_currentPlayerPosition.x < _playerPositionXMin)
            {
                _currentPlayerPosition.x = _playerPositionXMin;
            }

            if (_currentPlayerPosition.x > _playerPositionXMax)
            {
                _currentPlayerPosition.x = _playerPositionXMax;
            }

            if (_currentPlayerPosition.y < _playerPositionZMin)
            {
                _currentPlayerPosition.y = _playerPositionZMin;
            }

            if (_currentPlayerPosition.y > _playerPositionZMax)
            {
                _currentPlayerPosition.y = _playerPositionZMax;
            }

            _currentPlayerRotation = (_currentPlayerRotation + 6) % 6;

            _playerPositionBundObject.transform.localPosition = new Vector3(_currentPlayerPosition.x, 0f, _currentPlayerPosition.y);
            _playerPositionBundObject.transform.eulerAngles = new Vector3(0f, _currentPlayerRotation * 60f, 0f);
        }
    }
}