using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    // TODO temp types
    public enum TileElementType
    {
        typeA,
        typeB,
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private Transform _tileElementRoot;

        [SerializeField]
        private GameObject _canvasObject;

        private Dictionary<TileElementType, TileElement> _tileElements;

        private GameObject[] _groundPrefabs;
        private GameObject[] _treePrefabs;

        private int _groundIndex;
        private int _treeIndex;
        
        public void Initialize(Camera camera, GameObject[] groundPrefabs, GameObject[] treePrefabs)
        {
            _groundPrefabs = groundPrefabs;
            _treePrefabs = treePrefabs;

            _tileElements = new Dictionary<TileElementType, TileElement>();

            foreach(var tileElement in _tileElementRoot.GetComponentsInChildren<TileElement>())
            {
                _tileElements[tileElement.ElementType] = tileElement;
            }

            _canvasObject.GetComponent<Canvas>().worldCamera = camera;
            _canvasObject.transform.Find("Panel").Find("ButtonGround").GetComponent<Button>().onClick.AddListener(OnGroundButtonClicked);
            _canvasObject.transform.Find("Panel").Find("ButtonTree").GetComponent<Button>().onClick.AddListener(OnTreeButtonClicked);


            HideCanvas();
        }

        private void OnGroundButtonClicked()
        {
            _groundIndex = (_groundIndex + 1) % _groundPrefabs.Length;

            UpdateElementVisual(TileElementType.typeA, _groundPrefabs[_groundIndex]);
        }

        private void OnTreeButtonClicked()
        {
            _treeIndex = (_treeIndex + 1) % _treePrefabs.Length;

            UpdateElementVisual(TileElementType.typeB, _treePrefabs[_treeIndex]);
        }

       

        public void UpdateElementVisual(TileElementType elementType, GameObject visualPrefab)
        {
            if(_tileElements.TryGetValue(elementType, out var tileElement))
            { 
                tileElement.SetVisualWithInstantiation(visualPrefab);
            }
            else
            {
                Log.Error($"No tile element with type \"{elementType}\".");
            }
        }

        public void HideCanvas()
        {
            _canvasObject.SetActive(false);
        }

        public void ShowCanvas()
        {
            _canvasObject.SetActive(true);
        }
    }
}