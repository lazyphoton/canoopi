using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMInfo : InteractableMethod
    {
        public override string Text => "INFO";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Info"));

        [SerializeField]
        [TextArea(5, 20)]
        private string _infoText;

        public override void Interact()
        {
            base.Interact();

            World.GetService<UIManager>().PushUIInfo(_infoText);
        }
    }
}