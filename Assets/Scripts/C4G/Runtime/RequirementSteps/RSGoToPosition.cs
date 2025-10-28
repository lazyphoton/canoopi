using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSGoToPosition : IRequirementStep
    {
        public string Description => _description;

        private PlayerInformationManager _playerInformationManager;

        private Vector3 _position;
        private float _radius;
        private string _description;

        public RSGoToPosition(Vector3 position, float radius, string description)
        {
            _position = position;
            _radius = radius;
            _description = description;
        }

        public void OnStepStart()
        {
            _playerInformationManager = World.GetService<PlayerInformationManager>();
        }

        public bool IsRequirementMet()
        {
            return (_position - _playerInformationManager.CurrentPlayer.transform.position).magnitude < _radius;
        }

        public void OnStepComplete()
        {
            
        }
    }
}