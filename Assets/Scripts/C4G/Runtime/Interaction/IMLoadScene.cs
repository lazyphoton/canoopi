using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMLoadScene : InteractableMethod
    {
        [SerializeField]
        private GameSceneDefinition _scene;

        public override string Text => "SCENE";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Collect"));

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
            World.GetService<UIManager>().PushUILoadScene(_scene);
        }
    }
}