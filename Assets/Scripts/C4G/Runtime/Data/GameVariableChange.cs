using GameCore;
using System;
using UnityEngine;

namespace c4g
{
    [Serializable]
    public class GameVariableChange
    {
        [SerializeField]
        private GameVariableDefinition _gameVariableDefinition;

        [SerializeField]
        private string _value;

        public void ApplyChange()
        {
            var gameVariableManager = World.GetService<GameVariableManager>();

            if (_gameVariableDefinition == null)
            {
                Log.Error($"Null GameVariableDefinition in game variable change.");
                return;
            }

            Log.Debug($"Applying game variable change to \"{_gameVariableDefinition.VariableId}\", value: {_value}");

            gameVariableManager.SetValue(_gameVariableDefinition, _value);
        }
    }
}