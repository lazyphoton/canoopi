using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace c4g
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset _inputActions;

        [SerializeField]
        private Button _inputCatcherButton;

        private InputAction _clickAction;
        private InputAction _pointerMoveAction;

        public Vector2 CurrentPointerPosition => _pointerMoveAction.ReadValue<Vector2>();

        public event Action<Vector2> PointerClicked;

        public event Action<Vector2> CatchablePointerDown;
        public event Action<Vector2> CatchablePointerClicked;

        private void Start()
        {
            _clickAction = _inputActions.FindActionMap("Main").FindAction("Click");
            _clickAction.performed += OnClickPerformed;

            _pointerMoveAction = _inputActions.FindActionMap("Main").FindAction("PointerPos");

            _inputActions.Enable();
        }

        private void OnDestroy()
        {
            _clickAction.performed -= OnClickPerformed;
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            PointerClicked?.Invoke(_pointerMoveAction.ReadValue<Vector2>());
        }

        public void OnInputCatcherClicked()
        {
            CatchablePointerClicked?.Invoke(_pointerMoveAction.ReadValue<Vector2>());
        }

        public void OnInputCatcherPointerDown()
        {
            CatchablePointerDown?.Invoke(_pointerMoveAction.ReadValue<Vector2>());
        }
    }
}