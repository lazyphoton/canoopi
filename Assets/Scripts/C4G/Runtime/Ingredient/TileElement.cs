using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class TileElement : MonoBehaviour, IVisualizable
    {
        [SerializeField]
        private TileElementType _elementType;

        [SerializeField]
        private Transform _visualParentTransform;

        public TileElementType ElementType => _elementType;

        private GameObject _currentVisual;

        // TODO -> make into component system and avoid duplication of logic?
        public void SetVisual(GameObject visualObj)
        {
            if (_currentVisual != null)
                Destroy(_currentVisual);

            visualObj.transform.SetParent(_visualParentTransform);
            visualObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            visualObj.transform.localScale = Vector3.one;

            _currentVisual = visualObj;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}