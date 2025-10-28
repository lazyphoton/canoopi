using UnityEngine;
using UnityEngine.UIElements;

namespace c4g
{
    public class GridBranch : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int[] _snapPositions;

        [SerializeField]
        private Vector3 _snapOffset;

        [SerializeField]
        private GameObject _indicatorPrefab;

        [SerializeField]
        private GameObject _branchVisualObject;

        public Vector2Int[] SnapPositions => _snapPositions;
        public Vector3 SnapOffset => _snapOffset;


        private bool _isPlaced = false;

        public bool IsPlaced => _isPlaced;


        private Material _indicatorMaterial;

        private Color _incorrectColor = new Color(1f, 0.05f, 0f, 0.5f);
        private Color _correctColor = new Color(0f, 0.65f, 1f, 0.5f);

        private Vector3 _holdingOffset = Vector3.up * 0.4f;

        private void Start()
        {
            _indicatorMaterial = new Material(_indicatorPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial);

            foreach (var position in _snapPositions)
            {
                var indicatorObj = Instantiate(_indicatorPrefab, transform);
                indicatorObj.transform.localPosition = _snapOffset + new Vector3(position.x * BranchGrid.GridCellSize, 0f, position.y * BranchGrid.GridCellSize);
                indicatorObj.GetComponentInChildren<MeshRenderer>().sharedMaterial = _indicatorMaterial;
            }

            SetColorIncorrect();
            SetHolding(false);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetHolding(bool holding)
        {
            _branchVisualObject.transform.localPosition = holding ? _holdingOffset : Vector3.zero;
        }

        private void SetColorIncorrect()
        {
            _indicatorMaterial.color = _incorrectColor;
        }

        private void SetColorCorrect()
        {
            _indicatorMaterial.color = _correctColor;
        }

        public void SetPlaced(bool placed)
        {
            _isPlaced = placed;

            if (_isPlaced)
            {
                SetColorCorrect();
            }
            else
            {
                SetColorIncorrect();
            }
        }

        public Vector3 SnapToWorldPosition(Vector2Int snapPosition)
        {
            return transform.position + _snapOffset + new Vector3(snapPosition.x * BranchGrid.GridCellSize, 0f, snapPosition.y * BranchGrid.GridCellSize);
        }

        private void OnDrawGizmos()
        {
            if(_snapPositions == null || _snapPositions.Length == 0)
            {
                return;
            }

            foreach (var position in _snapPositions) 
            {
                var worldPos = SnapToWorldPosition(position);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(worldPos, BranchGrid.GridCellSize * 0.45f);
            }   
        }
    }
}