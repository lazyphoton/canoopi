using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMShowUI : InteractableMethod
    {
        [SerializeField]
        private UIFrameDefinition _uiFrameDefinition;

        [SerializeField]
        private UnityEngine.Object[] _objectsForFrame;

        public override string Text => "OPEN";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Collect"));

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
            World.GetService<UIManager>().PushUIShowUI(_uiFrameDefinition, _objectsForFrame);
        }
    }
}