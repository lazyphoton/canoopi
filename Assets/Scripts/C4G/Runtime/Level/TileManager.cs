using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace c4g
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tilePrefab;

        [SerializeField]
        private VisualData _visualData;

        private List<Tile> _tiles;

        private Player _player;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();

            var viewManager = await awaiter.AwaitServiceExistsAsync<ViewManager>();
            var mainCamera = viewManager.MainCamera;

            var playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();
            await awaiter.AwaitConditionAsync(() => playerInformationManager.CurrentPlayer != null);

            _player = playerInformationManager.CurrentPlayer;

            _player.Navigator.MovementStarted += OnPlayerStartMoving;
            _player.Navigator.MovementStopped += OnPlayerStopMoving;

            _tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None).ToList();

            foreach(var tile in _tiles)
            {
                tile.Initialize(
                        mainCamera,
                        new GameObject[] { _visualData.VisualPrefabs[0], _visualData.VisualPrefabs[1], _visualData.VisualPrefabs[2] },
                        new GameObject[] { _visualData.VisualPrefabs[0], _visualData.VisualPrefabs[3] });
            }
        }

        private void OnPlayerStartMoving()
        {
            foreach(var tile in _tiles)
            {
                tile.HideCanvas();
            }
        }

        private void OnPlayerStopMoving()
        {
            var playerPos = _player.gameObject.transform.position;

            foreach (var tile in _tiles)
            {
                if((playerPos - tile.gameObject.transform.position).magnitude < 5f)
                {
                    tile.ShowCanvas();
                }
            }
        }
    }
}