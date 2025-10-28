using Cinemachine;
using DG.Tweening;
using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class Player : MonoBehaviour, IVisualizable
    {
        public Navigator Navigator => _navigator;

        [SerializeField]
        private Transform _playerVisualTransform;

        [SerializeField]
        private CinemachineVirtualCamera _playerVirtualCamera;
        

        private ViewManager _viewManager;
        private PlayerInformationManager _playerInformationManager;
        private TimeManager _timeManager;

        private Navigator _navigator;
        private CharacterAnimator _characterAnimator;

        private GameObject _currentVisual;

        private bool _initialized = false;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            if (_initialized) 
            {
                Log.Error("Attempting to re-initialize player.");
                return; 
            }

            var awaiter = World.GetService<Awaiter>();

            _viewManager = await awaiter.AwaitServiceExistsAsync<ViewManager>();
            _playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();
            _timeManager = await awaiter.AwaitServiceExistsAsync<TimeManager>();
            var uiManager = await awaiter.AwaitServiceExistsAsync<UIManager>();

            _viewManager.SetCameraAsPriority(_playerVirtualCamera);

            _navigator = GetComponent<Navigator>();
            _navigator.MovementStarted += OnMovementStarted;
            _navigator.MovementStopped += OnMovementStopped;

            _characterAnimator = GetComponent<CharacterAnimator>();

            _initialized = true;
            _playerInformationManager.CurrentPlayer = this;

            if(_playerInformationManager.CurrentPlayerInventory == null)
            {
                _playerInformationManager.CurrentPlayerInventory = new Inventory();
            }

            _playerInformationManager.ResetKeyValues();

            // Potential timing issues with UI manager initialization
            _timeManager.DoAfterSeconds(() => { uiManager.PushUICharacterSelect(); }, 0.25f);
        }

        private void OnDestroy()
        {
            _playerInformationManager.ResetKeyValues();
            _playerInformationManager.CurrentPlayer = null;
        }

        private void Update()
        {
            if(!_initialized)
                return;
        }

        private void OnMovementStarted()
        {
            
        }

        private void OnMovementStopped()
        {
            
        }

        public void SetVisual(GameObject visualObj)
        {
            if(_currentVisual != null)
                Destroy(_currentVisual);

            visualObj.transform.SetParent(_playerVisualTransform);
            visualObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            visualObj.transform.localScale = Vector3.one;

            _currentVisual = visualObj;
        }
    }
}