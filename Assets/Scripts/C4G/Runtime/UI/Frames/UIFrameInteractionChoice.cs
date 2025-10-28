using GameCore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameInteractionChoice : AUIFrame
    {
        public const string UIKEY_InteractableMethods = "UIFrameInteractionChoiceInteractableMethods";
        public const string UIKEY_HitPosition = "UIFrameInteractionChoiceHitPosition";

        [SerializeField]
        private GameObject _interactionButtonPrefab;

        [SerializeField]
        private Button _catcherButton;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _catcherButton.onClick.AddListener(OnCatcherButtonClicked);

            var currentInteractableMethods = (List<IInteractableMethod>)injectedInfo[UIKEY_InteractableMethods];
            var interactablePosition = (Vector3)injectedInfo[UIKEY_HitPosition];

            var mainCamera = World.GetService<ViewManager>().MainCamera;
            var screenPos = mainCamera.WorldToScreenPoint(interactablePosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPos, null, out var anchoredPos);

            

            for(int i = 0; i < currentInteractableMethods.Count; i++)
            {
                var method = currentInteractableMethods[i];

                var angle = (Mathf.PI / 2f) + ((-i + ((currentInteractableMethods.Count-1f)/ 2f)) * 0.65f);
                var offsetPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 170f;

                var buttonObj = Instantiate(_interactionButtonPrefab, transform);
                buttonObj.GetComponent<RectTransform>().anchoredPosition = anchoredPos + offsetPos;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => { OnInteractionButtonClicked(method); });
                //buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = method.Text;
                buttonObj.GetComponent<Image>().sprite = method.Icon;
            }
        }

        private void OnInteractionButtonClicked(IInteractableMethod method)
        {
            PopSelf();
            method.Interact();
        }

        private void OnCatcherButtonClicked()
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