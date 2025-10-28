using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSWaitForKeyValue : ARequirementStep
    {
        public override string Description => _description;

        private PlayerInformationManager _playerInformationManager;

        [Header("Wait For Key Value")]
        [SerializeField]
        private string _description;

        [SerializeField]
        private string _key;

        public override void OnStepStart()
        {
            _playerInformationManager = World.GetService<PlayerInformationManager>();
        }

        public override bool IsRequirementMet()
        {
            if(_playerInformationManager.TryGetKeyValue<bool>(_key, out var value))
            {
                return value;
            }

            return false;
        }
    }
}