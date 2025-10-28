using GameCore;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class StartupInitializer : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();

            var gameVariableManager = await awaiter.AwaitServiceExistsAsync<GameVariableManager>();
            var playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();

            // Completely reset player informaiton manager and game variables when stargint via the start scene
            World.Services.OverwriteService(typeof(GameVariableManager), new GameVariableManager(), true);
            World.Services.OverwriteService(typeof(PlayerInformationManager), new PlayerInformationManager(), true);
        }
    }
}