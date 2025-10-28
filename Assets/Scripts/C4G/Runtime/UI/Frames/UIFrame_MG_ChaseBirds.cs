using GameCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_ChaseBirds : UIFrameMinigame
    {
        public const int STATE_STARTED = 5;
        public const int STATE_FINISHING = 10;
        public const int STATE_FINISHED = 15;

        [Header("Bird Chasing")]
        [SerializeField]
        private DialogPartDefinition _dialogStart;

        [SerializeField]
        private DialogPartDefinition _dialogEnd;

        [SerializeField]
        private GameObject _birdPrefab;

        private Player _player;

        private GameObject _dialogCamGroupObject;

        private List<Vector3> _bundLocations;

        private int _birdsScaredOff = 0;
        private int _totalBirds = 20;

        private int _maxActiveBirds = 12;

        private List<BundBird> _activeBirds = new List<BundBird>();

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();

            var levelLayoutObject = GameObject.Find("LevelLayout_MG_ChaseBirds");

            _dialogCamGroupObject = levelLayoutObject.transform.Find("DialogCameraGroup").gameObject;

            var bundsParent = levelLayoutObject.transform.Find("Bunds");
            _bundLocations = new List<Vector3>();

            foreach(Transform child in bundsParent)
            {
                _bundLocations.Add(child.position);
            }

            _interactionManager.PointerClickRaycastHit += OnPointerRaycastHit;

            // Do start dialog first, wait for other panels to load
            _timeManager.DoAfterShortDelay(() => { UiManager.PushUIDialog(_dialogStart, _dialogCamGroupObject); });
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            _interactionManager.PointerClickRaycastHit -= OnPointerRaycastHit;

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
                    UpdateChase();
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

        private void UpdateChase()
        {
            if(_player == null)
            {
                _player = _playerInformationManager.CurrentPlayer;

                var navmeshAgent = _player.GetComponentInChildren<NavMeshAgent>();
                navmeshAgent.speed = 7f;
            }

            UpdateBirds();

            if (_birdsScaredOff >= _totalBirds)
            {
                SetMinigameState(STATE_FINISHING);
                UiManager.PushUIDialog(_dialogEnd, _dialogCamGroupObject);
            }
        }

        private void UpdateBirds()
        {
            if (_activeBirds.Count < _maxActiveBirds && _activeBirds.Count + _birdsScaredOff < _totalBirds)
            {
                if(UnityEngine.Random.Range(0f, 1f) < 0.05f)
                {
                    var birdObj = Instantiate(_birdPrefab);
                    var bundBird = birdObj.GetComponent<BundBird>();
                    bundBird.Initialize(_player.gameObject, _bundLocations);
                    _activeBirds.Add(bundBird);
                }
            }

            var birdsToRemove = new List<BundBird>();

            foreach(var bird in _activeBirds)
            {
                if (bird.Scared)
                {
                    birdsToRemove.Add(bird);
                    _birdsScaredOff++;
                }
            }

            foreach(var bird in birdsToRemove)
            {
                _activeBirds.Remove(bird);
            }
        }

        private void OnPointerRaycastHit(RaycastHit hitInfo)
        {
            if (_player == null)
            {
                return;
            }

            var hitLayer = hitInfo.collider.gameObject.layer;

            if (hitLayer == LayerMask.NameToLayer("Floor"))
            {
                _player.Navigator.SetTargetDestination(hitInfo.point);
            }
        }
    }
}