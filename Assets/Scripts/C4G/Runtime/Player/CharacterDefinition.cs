using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(menuName = "C4G/Character Definition")]
    public class CharacterDefinition : ScriptableObject
    {
        [SerializeField]
        private string _characterName;

        [SerializeField]
        private Sprite _characterHeadshot;


        public string CharacterName => _characterName;

        public Sprite CharacterHeadshot => _characterHeadshot;
    }
}