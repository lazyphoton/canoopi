using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMCollect : InteractableMethod
    {
        [SerializeField]
        private ItemDefinition _itemDefinition;

        [SerializeField]
        [Range(1, 10)]
        private int _amount;

        public override string Text => "COLLECT";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Collect"));

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
            PlayerInformationManager.CurrentPlayerInventory.AddItem(_itemDefinition, _amount);

            Destroy(gameObject);
        }
    }
}