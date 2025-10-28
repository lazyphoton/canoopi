using GameCore;
using UnityEngine;

namespace c4g
{
    public class SetGameVariablesOnLoad : MonoBehaviour
    {
        [SerializeField]
        private GameVariableChange[] _changes;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();
            await awaiter.AwaitServiceExistsAsync<GameVariableManager>();

            foreach (var change in _changes)
            {
                change.ApplyChange();
            }
        }
    }
}