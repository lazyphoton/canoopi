using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace c4g
{
    public class InteractionManager
    {
        public event Action<RaycastHit> PointerClickRaycastHit;
        public event Action<RaycastHit> PointerDownRaycastHit;

        private ViewManager _viewManager;
        private InputManager _inputManager;
        private PlayerInformationManager _playerInformationManager;
        private UIManager _uiManager;

        private bool _initialized = false;

        public InteractionManager()
        {
            Initialize();
        }

        private async void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            var awaiter = World.GetService<Awaiter>();

            _viewManager = await awaiter.AwaitServiceExistsAsync<ViewManager>();
            _inputManager = await awaiter.AwaitServiceExistsAsync<InputManager>();
            _playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();
            _uiManager = await awaiter.AwaitServiceExistsAsync<UIManager>();

            _inputManager.CatchablePointerDown += OnCatchablePointerDown;
            _inputManager.CatchablePointerClicked += OnCatchablePointerClicked;
        }

        private void OnCatchablePointerDown(Vector2 pointerPos)
        {
            if (TryRaycastFromPointerPosition(pointerPos, out var hitInfo))
            {
                PointerDownRaycastHit?.Invoke(hitInfo);
            }
        }

        private void OnCatchablePointerClicked(Vector2 pointerPos)
        {
            if (TryRaycastFromPointerPosition(pointerPos, out var hitInfo))
            {
                PointerClickRaycastHit?.Invoke(hitInfo);
            }
        }

        private bool TryRaycastFromPointerPosition(Vector2 pointerPos, out RaycastHit hitInfo)
        {
            var ray = _viewManager.MainCamera.ScreenPointToRay(pointerPos);

            if (Physics.Raycast(ray, out hitInfo))
            {
                return true;
            }

            return false;
        }

        public Ray GetCurentPointerRay()
        {
            return _viewManager.MainCamera.ScreenPointToRay(_inputManager.CurrentPointerPosition);
        }
    }
}