using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c4g
{
    public class RSGoToObjectWithTag : ARequirementStep
    {
        public override string Description => _description;

        private PlayerInformationManager _playerInformationManager;

        [Header("Go To Object With Tag")]
        [SerializeField]
        private string _tag;

        [SerializeField]
        private float _radius;

        [SerializeField]
        private string _description;

        private GameObject[] _taggedObjects;

        public override void OnStepStart()
        {
            _playerInformationManager = World.GetService<PlayerInformationManager>();
            _taggedObjects = GameObject.FindGameObjectsWithTag(_tag);
        }

        public override bool IsRequirementMet()
        {
            foreach (var obj in _taggedObjects) 
            {
                if ((obj.transform.position - _playerInformationManager.CurrentPlayer.transform.position).magnitude < _radius)
                {
                    return true;
                }
            }

            return false;
        }
    }
}