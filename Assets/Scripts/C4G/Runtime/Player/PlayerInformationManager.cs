using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class PlayerInformationManager
    {
        public Player CurrentPlayer { get; set; }

        public Inventory CurrentPlayerInventory { get; set; }

        public int CurrentPlayerVisualIndex = -1;
        public bool HasPlayerChosenVisual => CurrentPlayerVisualIndex >= 0;

        public QuestState CurrentQuest { get; set; }

        private Dictionary<string, object> _keyValues = new Dictionary<string, object>();

        public void SetKeyValue(string key, object value)
        {
            _keyValues[key] = value;
        }

        public void ResetKeyValues()
        {
            _keyValues.Clear();
        }

        public bool TryGetKeyValue<T>(string key, out T value)
        {
            value = default(T);

            if(_keyValues.TryGetValue(key, out var val))
            {
                value = (T)val;
                return true;
            }

            return false;
        }
    }
}