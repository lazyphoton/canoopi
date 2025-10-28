using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameInventory : AUIFrame
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private TextMeshProUGUI _infoText;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            var sb = new StringBuilder();
            sb.Append("Inventory Contents: (placeholder)\n\n");

            var inventoryItems = World.GetService<PlayerInformationManager>().CurrentPlayerInventory.AllItems;

            foreach (var item in inventoryItems) 
            {
                sb.AppendLine($"{item.Definition.ItemName} : {item.Amount}");
            }

            _infoText.text = sb.ToString();

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