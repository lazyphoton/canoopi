using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(menuName = "C4G/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField]
        private string _itemName;

        [SerializeField]
        private int _iconIndex = -1;

        public string ItemName => _itemName;

        public int IconIndex => _iconIndex;
    }
}