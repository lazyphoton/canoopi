using GameCore;
using UnityEngine;

namespace c4g
{
    public class UIFrameDefinitionHolder : MonoBehaviour
    {
        [SerializeField]
        private UIFrameDefinition _loadSceneFrameDefinition;

        [SerializeField]
        private UIFrameDefinition _dialogFrameDefinition;

        [SerializeField]
        private UIFrameDefinition _infoTextFrameDefinition;

        [SerializeField]
        private UIFrameDefinition _characterSelectFrameDefinition;

        [SerializeField]
        private UIFrameDefinition _interactionChoiceFrameDefinition;


        public UIFrameDefinition LoadSceneFrameDefinition => _loadSceneFrameDefinition;
        public UIFrameDefinition DialogFrameDefinition => _dialogFrameDefinition;
        public UIFrameDefinition InfoTextFrameDefinition => _infoTextFrameDefinition;
        public UIFrameDefinition CharacterSelectFrameDefinition => _characterSelectFrameDefinition;
        public UIFrameDefinition InteractionChoiceFrameDefinition => _interactionChoiceFrameDefinition;



        private void Start()
        {
            UIManagerExtensions.SetUIFrameDefinitionHolderInstance(this);
        }
    }
}