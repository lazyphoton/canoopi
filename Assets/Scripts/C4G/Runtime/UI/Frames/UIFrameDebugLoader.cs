using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameDebugLoader : AUIFrame
    {
        [SerializeField]
        private Button _debugButton;

        [SerializeField]
        private UIFrameDefinition _debugFrameDefinition;

        private bool _visible;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SetVisibility(!_visible);
            }
        }

        private void SetVisibility(bool visible)
        {
            _visible = visible;

            _debugButton.gameObject.SetActive(_visible);
        }

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _debugButton.onClick.AddListener(OnDebugButtonClicked);
            SetVisibility(false);
        }

        private void OnDebugButtonClicked()
        {
            UiManager.PushUI(_debugFrameDefinition);
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}