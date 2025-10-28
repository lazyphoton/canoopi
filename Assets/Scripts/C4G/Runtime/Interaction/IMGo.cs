using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMGo : InteractableMethod
    {
        public override string Text => "GO";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Go"));

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
        }
    }
}