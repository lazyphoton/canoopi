using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class InventoryItem
    {
        public ItemDefinition Definition { get; private set; }
        public int Amount { get; private set; }

        public InventoryItem(ItemDefinition definition) : this(definition, 1) { }

        public InventoryItem(ItemDefinition definition, int amount)
        {
            Definition = definition;
            Amount = amount;
        }

        public void AddAmount(int amount)
        {
            Amount += amount;
        }
    }
}