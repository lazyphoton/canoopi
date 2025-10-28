using GameCore;
using UnityEngine;

namespace c4g
{
    public class ActivateOnCondition : MonoBehaviour
    {
        [SerializeField]
        private GameObject _targetObject;

        [SerializeField]
        private GCGameVariable _gameVariableCondition;

        private GameVariableManager _gameVariableManager;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();
            _gameVariableManager = await awaiter.AwaitServiceExistsAsync<GameVariableManager>();
            _gameVariableManager.GameVariableChanged += OnGameVariableChanged;

            CheckCondition();
        }

        private void OnDestroy()
        {
            _gameVariableManager.GameVariableChanged -= OnGameVariableChanged;
        }

        private void OnGameVariableChanged(GameVariableDefinition gameVariableDefintion)
        {
            if(_gameVariableCondition == null)
            {
                Log.Error("Game variable condition is null.");
                return;
            }

            if(gameVariableDefintion == _gameVariableCondition.GameVariableDefinition)
            {
                CheckCondition();
            }
        }

        private void CheckCondition()
        {
            if(_targetObject == null)
            {
                Log.Error("Object for activation is null.");
                return;
            }

            _targetObject.SetActive(_gameVariableCondition.IsConditionMet());
        }
    }
}
