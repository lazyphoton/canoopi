using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace c4g
{
    public class Inventory
    {
        public event Action<Inventory> InventoryUpdated;

        public List<InventoryItem> AllItems => _items.Values.ToList();

        private Dictionary<ItemDefinition, InventoryItem> _items;

        public Inventory()
        {
            _items = new Dictionary<ItemDefinition, InventoryItem>();
        }

        public void AddItem(ItemDefinition definition, int amount)
        {
            if (definition == null) 
            {
                Log.Error("Null item definition not allowed.");
                return;
            }

            // TODO: removing items?
            if(amount <= 0)
            {
                Log.Error("Removing inventory items not currently supported.");
                return;
            }

            if(_items.TryGetValue(definition, out var item))
            {
                item.AddAmount(amount);
            }
            else
            {
                _items[definition] = new InventoryItem(definition, amount);
            }

            InventoryUpdated?.Invoke(this);
        }

        public int GetAmount(ItemDefinition definition)
        {
            if(_items.TryGetValue(definition, out var item))
            {
                return item.Amount;
            }

            return 0;
        }

        public void Empty()
        {
            _items.Clear();

            InventoryUpdated?.Invoke(this);
        }
    }
}