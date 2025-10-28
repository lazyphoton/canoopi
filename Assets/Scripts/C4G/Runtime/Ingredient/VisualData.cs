using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(menuName = "C4G/Visual Data")]
    public class VisualData : ScriptableObject
    {
        [SerializeField]
        private GameObject[] _visualPrefabs;

        public GameObject[] VisualPrefabs => _visualPrefabs;
    }
}