using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSESetKeyValue : IRequirementStepEvent
    {
        [Header("Set Key Value")]
        [SerializeField]
        private string _key;

        [SerializeField]
        private string _value;

        public void Trigger()
        {
            World.GetService<PlayerInformationManager>().SetKeyValue( _key, _value );
        }
    }
}