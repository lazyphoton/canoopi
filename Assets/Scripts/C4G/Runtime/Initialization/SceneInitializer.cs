using GameCore;
using GameGore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace c4g
{
    public class SceneInitializer : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private UIFrameDefinition _initialUIFrameDefinition;

        [SerializeField]
        private UIFrameDefinition _debugLoaderUIFrameDefinition;

        [Header("Player")]

        [SerializeField]
        private GameObject _playerPrefab;

        private void Start()
        {
            World.GetService<ISceneManager>().LoadSceneAdditiveAsync("CommonSystems");

            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();
            var uiManager = await awaiter.AwaitServiceExistsAsync<UIManager>();
            uiManager.SetUI(_initialUIFrameDefinition, new Dictionary<string, object>());

            // Load the debug loader panel automatically
            // Comment this to disable the debug panel in game
            uiManager.PushUI(_debugLoaderUIFrameDefinition);

            SpawnPlayer();

            uiManager.DoSceneTransitionShow();
        }

        private void SpawnPlayer()
        {
            var playerSpawnPoints = FindObjectsByType<PlayerSpawnPoint>(FindObjectsSortMode.None)
                .OrderByDescending(x => x.Priority)
                .ToList();

            if (playerSpawnPoints.Count == 0)
            {;
                Log.Debug("No player spawnpoints found.");
                return;
            }

            if(_playerPrefab == null)
            {
                Log.Error("Trying to spawn null player prefab.");
                return;
            }

            var spawnPos = playerSpawnPoints[0].SpawnPosition;
            var spawnRot = playerSpawnPoints[0].SpawnRotation;

            var playerObj = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);

            var lookDirection = Quaternion.Euler(0, spawnRot, 0) * Vector3.forward;

            World.GetService<TimeManager>().DoAfterShortDelay(() =>
            {
                World.GetService<PlayerInformationManager>().CurrentPlayer.Navigator.LookAtTarget(spawnPos + lookDirection);
            });
        }
    }
}