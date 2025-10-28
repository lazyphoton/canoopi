using GameCore;
using UnityEngine;

namespace c4g
{
    public class RSESetGameVariable : IRequirementStepEvent
    {
        [SerializeField]
        private GameVariableChange _gameVariableChange;

        public void Trigger()
        {
            if (_gameVariableChange == null)
            {
                Log.Error($"Null GameVariable Change in requirement step event.");
                return;
            }

            _gameVariableChange.ApplyChange();
        }
    }
}