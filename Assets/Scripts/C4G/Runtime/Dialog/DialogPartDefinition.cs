using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(fileName = "NewDialogPart", menuName = "C4G/DialogPartDefinition")]
    public class DialogPartDefinition : ScriptableObject
    {
        [SerializeField]
        private CharacterDefinition _characterDefinition;

        [SerializeField]
        private DialogHeadshotPosition _headshotPosition;

        [SerializeField]
        private List<GameVariableChange> _gameVariablesToSetOnPartStart = new List<GameVariableChange>();

        [SerializeField]
        [TextArea(5, 20)]
        private string _dialogText;

        [SerializeField]
        private List<GameVariableChange> _gameVariablesToSetOnPartEnd = new List<GameVariableChange>();

        [SerializeField]
        private DialogPartDefinition _defaultNextDialogPart;

        [Tooltip("Conditions are checked in the order of the list.\nWhichever is true first will be the next step, otherwise default.")]
        [SerializeField]
        private List<NextDialogPartCondition> _conditionalNextDialogParts;


        public CharacterDefinition CharacterDefinition => _characterDefinition;
        public DialogHeadshotPosition HeadshotPosition => _headshotPosition;
        public List<GameVariableChange> GameVariablesToSetOnPartStart => _gameVariablesToSetOnPartStart;
        public string DialogText => _dialogText;
        public List<NextDialogPartCondition> ConditionalNextDialogParts => _conditionalNextDialogParts;
        public DialogPartDefinition DefaultNextDialogPart => _defaultNextDialogPart;
        public List<GameVariableChange> GameVariablesToSetOnPartEnd => _gameVariablesToSetOnPartEnd;

        public void AddConditionlNextDialogPartByType(Type conditionType)
        {
            var conditionObj = (IGameCondition)Activator.CreateInstance(conditionType);
            var nextConditionalObj = new NextDialogPartCondition(conditionObj);
            
            _conditionalNextDialogParts.Add(nextConditionalObj);
        }
    }
}