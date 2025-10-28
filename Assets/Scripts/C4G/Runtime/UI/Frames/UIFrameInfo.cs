using GameCore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameInfo : AUIFrame
    {
        public const string UIKEY_InfoText = "UIFrameInfoText";

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private TextMeshProUGUI _infoText;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            var text = (string)injectedInfo[UIKEY_InfoText];
            _infoText.text = text;

            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            PopSelf();
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {
            
        }
    }
}