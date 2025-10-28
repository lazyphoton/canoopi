using GameCore;
using System;
using UnityEngine;

namespace c4g
{
    [Serializable]
    public class GCCollectItem : IGameCondition
    {
        [Header("Collect Item Condition")]
        [SerializeField]
        private ItemDefinition _itemDefinition;

        [SerializeField]
        private int _amount;

        public ItemDefinition ItemDefinition => _itemDefinition;
        public int Amount => _amount;

        public bool IsConditionMet()
        {
            var playerInformationManager = World.GetService<PlayerInformationManager>();

            return playerInformationManager.CurrentPlayerInventory.GetAmount(_itemDefinition) >= _amount;
        }
    }
}