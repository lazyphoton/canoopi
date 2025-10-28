using GameCore;
using UnityEngine;

namespace c4g
{
    public class RSWaitForGameVariable : ARequirementStep
    {
        public override string Description => _description;

        private GameVariableManager _gameVariableManager;

        [SerializeField]
        private string _description;

        [SerializeField]
        private GCGameVariable _gameVariableCondition;

        public override void OnStepStart()
        {
            _gameVariableManager = World.GetService<GameVariableManager>();
        }

        public override bool IsRequirementMet()
        {
            if(_gameVariableCondition == null)
            {
                Log.Error($"Null condition in requirement step: {Description}");
                return false;
            }

            return _gameVariableCondition.IsConditionMet();
        }
    }
}