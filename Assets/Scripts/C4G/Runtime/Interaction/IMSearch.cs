using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMSearch : InteractableMethod
    {
        public override string Text => "SEARCH";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Search"));

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
            World.GetService<UIManager>().PushUIInfo("Search interaction triggered: not implemented.");
        }
    }
}