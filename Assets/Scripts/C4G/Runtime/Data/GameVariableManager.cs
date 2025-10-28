using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class GameVariableManager
    {
        public event Action<GameVariableDefinition> GameVariableChanged;

        private Dictionary<string, Type> _types;
        private Dictionary<string, object> _keyValues;
        private Dictionary<string, GameVariableDefinition> _variableDefinitions;

        public GameVariableManager()
        {
            _types = new Dictionary<string, Type>();
            _keyValues = new Dictionary<string, object>();
            _variableDefinitions = new Dictionary<string, GameVariableDefinition>();

            var gameVariableDefinitions = Resources.LoadAll<GameVariableDefinition>("GameVariables");

            foreach(var gameVariableDefinition in gameVariableDefinitions)
            {
                if (!IsVariableIdValid(gameVariableDefinition.VariableId))
                {
                    Log.Error($"Invalid GameVariable Id: \"{gameVariableDefinition.VariableId}\"");
                    continue;
                }

                if(_keyValues.TryGetValue(gameVariableDefinition.VariableId, out _))
                {
                    Log.Error($"Duplicate GameVariable Id: \"{gameVariableDefinition.VariableId}\"");
                    continue;
                }

                Type type;
                object value;

                switch (gameVariableDefinition.VariableType)
                {
                    case GameVariableType.Bool:
                        type = typeof(bool);
                        value = false;
                        break;
                    case GameVariableType.Int:
                        type = typeof(int);
                        value = 0;
                        break;
                    case GameVariableType.String:
                        type = typeof(string); 
                        value = string.Empty;
                        break;
                    default:
                        Log.Error($"Unrecognized GameVariable type: {gameVariableDefinition.VariableType}");
                        continue;
                }

                _types[gameVariableDefinition.VariableId] = type;
                _keyValues[gameVariableDefinition.VariableId] = value;
                _variableDefinitions[gameVariableDefinition.VariableId] = gameVariableDefinition;
            }
        }

        private bool IsVariableIdValid(string variableId)
        {
            if (string.IsNullOrEmpty(variableId))
            {
                return false;
            }

            if(variableId.Contains(" ", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            value = default(T);

            if (_keyValues.TryGetValue(key, out var val) && _types.TryGetValue(key, out var type)) 
            {
                if(type != typeof(T))
                {
                    Log.Error($"Invalid type ({typeof(T)}) for GameVariable \"{key}\", expected {type}");
                    return false;
                }

                value = (T)val;
                return true;
            }
            else
            {
                Log.Error($"No GameValue with key \"{key}\"");
            }

            return false;
        }

        public void SetValue<T>(string key, T value)
        {
            if (_keyValues.TryGetValue(key, out var val) && _types.TryGetValue(key, out var type))
            {
                if (type != typeof(T))
                {
                    Log.Error($"Invalid type ({typeof(T)}) for GameVariable \"{key}\", expected {type}");
                    return;
                }

                _keyValues[key] = value;

                GameVariableChanged?.Invoke(_variableDefinitions[key]);
            }
            else
            {
                Log.Error($"No GameValue with key \"{key}\"");
            }
        }
    }
}