using Cinemachine;
using GameCore;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrameLibrary : AUIFrame
    {
        [SerializeField]
        private GameObject _challengeButtonPrefab;

        [SerializeField]
        private Transform _challengeButtonParentTransform;

        [SerializeField]
        private GameObject _descriptionPanel;

        [SerializeField]
        private TMP_Text _descriptionText;

        [SerializeField]
        private TMP_Text _titleText;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private EcologicalChallengeData[] _challenges;

        private MeshRenderer _earthOverlayRenderer;
        private Material _defaultOverlayMaterial;

        private ViewManager _viewManager;


        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            _viewManager = World.GetService<ViewManager>();

            var sceneObjects = (UnityEngine.Object[])injectedInfo[UIManagerExtensions.UIKEY_InjectedSceneObjects];

            var earthVirtualCam = ((GameObject)sceneObjects[1]).GetComponent<CinemachineVirtualCamera>();

            _viewManager.PushCamera(earthVirtualCam);

            _descriptionPanel.SetActive(false);

            var earthOverlayObj = (GameObject)sceneObjects[0];
            _earthOverlayRenderer = earthOverlayObj.GetComponent<MeshRenderer>();
            _defaultOverlayMaterial = _earthOverlayRenderer.material;

            for (int i = 0; i < _challenges.Length; i++) 
            {
                var index = i;

                var button = Instantiate(_challengeButtonPrefab);
                button.name = $"ChallengeButton_{index}";
                button.transform.SetParent(_challengeButtonParentTransform);
                button.transform.localScale = Vector3.one;

                button.GetComponentInChildren<TMP_Text>().text = _challenges[index].buttonLabel;
                button.GetComponent<Button>().onClick.AddListener(() => ShowOverlay(_challenges[index]));
            }

            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            _viewManager.PopCamera();
            _earthOverlayRenderer.material = _defaultOverlayMaterial;
            PopSelf();
        }

        private void ShowOverlay(EcologicalChallengeData challengeData)
        {
            _descriptionPanel.SetActive(true);
            _descriptionText.text = challengeData.description;
            _titleText.text = challengeData.title;

            _earthOverlayRenderer.material = challengeData.overlayMaterial;
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }
    }
}