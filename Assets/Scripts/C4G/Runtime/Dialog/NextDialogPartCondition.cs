using System;
using UnityEngine;

namespace c4g
{
    [Serializable]
    public class NextDialogPartCondition
    {
        [SerializeReference]
        private IGameCondition _gameCondition;

        [SerializeField]
        private DialogPartDefinition _nextDialogPart;

        public IGameCondition GameCondition => _gameCondition;
        public DialogPartDefinition NextDialogPart => _nextDialogPart;

        public NextDialogPartCondition() { }

        public NextDialogPartCondition(IGameCondition gameCondition) 
        { 
            _gameCondition = gameCondition;
        }
    }
}