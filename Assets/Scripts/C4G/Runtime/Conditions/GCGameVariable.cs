using GameCore;
using System;
using UnityEngine;

namespace c4g
{
    public enum GameVariableConditionType
    {
        Equal = 0,
        GreaterThanOrEqual = 1
    }

    [Serializable]
    public class GCGameVariable : IGameCondition
    {
        [Header("GameVariable Condition")]
        [SerializeField]
        private GameVariableDefinition _gameVariableDefinition;

        [SerializeField]
        public GameVariableConditionType _conditionType = GameVariableConditionType.Equal;

        [SerializeField]
        private string _targetValue;

        private GameVariableManager _gameVariableManager;

        public GameVariableDefinition GameVariableDefinition => _gameVariableDefinition;

        public string TargetValue => _targetValue;

        public bool IsConditionMet()
        {
            _gameVariableManager = World.GetService<GameVariableManager>();

            if (_gameVariableDefinition == null)
            {
                Log.Error($"Null GameVariableDefinition in condition.");
                return false;
            }

            switch (_gameVariableDefinition.VariableType)
            {
                case GameVariableType.Bool: return IsBoolConditionMet();
                case GameVariableType.Int: return IsIntConditionMet();
                case GameVariableType.String: return IsStringConditionMet();
            }

            Log.Error($"No condition check for GameVariable type: {_gameVariableDefinition.VariableType}");

            return false;
        }

        private bool IsBoolConditionMet()
        {
            if (_gameVariableManager.TryGetBool(_gameVariableDefinition.VariableId, out var value))
            {
                if (bool.TryParse(_targetValue, out var result))
                {
                    return value == result;
                }

                Log.Error($"Could not parse \"{_targetValue}\" into bool.");
            }

            return false;
        }

        private bool IsIntConditionMet()
        {
            if (_gameVariableManager.TryGetInt(_gameVariableDefinition.VariableId, out var currentVarValue))
            {
                if (int.TryParse(_targetValue, out var parsedTargetValue))
                {
                    switch (_conditionType)
                    {
                        case GameVariableConditionType.GreaterThanOrEqual:
                            return currentVarValue >= parsedTargetValue;
                        case GameVariableConditionType.Equal:
                        default:
                            return currentVarValue == parsedTargetValue;
                    }
                }

                Log.Error($"Could not parse \"{_targetValue}\" into int.");
            }

            return false;
        }

        private bool IsStringConditionMet()
        {
            if (_gameVariableManager.TryGetString(_gameVariableDefinition.VariableId, out var value))
            {
                return value.Equals(_targetValue);
            }

            return false;
        }
    }
}