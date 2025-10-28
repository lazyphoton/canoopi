using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class BranchGrid : MonoBehaviour
    {
        public static readonly float GridCellSize = 0.5f;

        private GridBranch[] _gridBranches;

        private Dictionary<Vector2Int, GridBranch> _gridCells;

        private void Start()
        {
            _gridBranches = FindObjectsByType<GridBranch>(FindObjectsSortMode.None);

            _gridCells = new Dictionary<Vector2Int, GridBranch>();

            foreach(var position in GetGridPositions())
            {
                _gridCells[position] = null;
            }
        }

        public bool IsGridComplete()
        {
            foreach (var branch in _gridBranches) 
            {
                if (!branch.IsPlaced)
                {
                    return false;
                }
            }

            return true;
        }

        public void TryPlaceBranch(GridBranch branch)
        {
            var snapPositions = branch.SnapPositions;

            var testSnapPosition = branch.SnapToWorldPosition(Vector2Int.zero);
            var closestGridPosition = GetClosestGridPosition(testSnapPosition);

            var positionsToOccupy = new Vector2Int[snapPositions.Length];

            for(int i = 0; i < snapPositions.Length; i++)
            {
                positionsToOccupy[i] = closestGridPosition + snapPositions[i];
            }

            if(IsPlaceAvailable(positionsToOccupy))
            {
                foreach(var positionToOccupy in positionsToOccupy)
                {
                    _gridCells[positionToOccupy] = branch;
                }

                branch.SetPosition(GridToWorldPosition(closestGridPosition) - branch.SnapOffset);
                branch.SetPlaced(true);
            }
            else
            {
                branch.SetPlaced(false);
            }
        }

        public void UnoccupyBranch(GridBranch branch)
        {
            var cellsToNull = new List<Vector2Int>();

            foreach(var kvp in _gridCells)
            {
                if(kvp.Value == branch)
                {
                    cellsToNull.Add(kvp.Key);
                }
            }

            foreach(var cell in cellsToNull)
            {
                _gridCells[cell] = null;
            }
        }

        private bool IsPlaceAvailable(Vector2Int[] gridPositions)
        {
            foreach (var gridPosition in gridPositions) 
            {
                if(!_gridCells.TryGetValue(gridPosition, out var occupyingBranch) || occupyingBranch != null)
                {
                    return false;
                }
            }

            return true;
        }

        private Vector2Int GetClosestGridPosition(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt((worldPosition.x / GridCellSize) + 0.5f),
                Mathf.FloorToInt((worldPosition.z / GridCellSize) + 0.5f));
        }

        public List<Vector2Int> GetGridPositions()
        {
            var positions = new List<Vector2Int>();

            for(int i = -3; i < 4; i++)
            {
                for(int j = 0; j < Mathf.Min(4, 5 - Mathf.Abs(i)); j++)
                {
                    positions.Add(new Vector2Int(i, j));
                }
            }


            return positions;
        }

        public Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x * GridCellSize, 0f, gridPosition.y * GridCellSize);
        }

        private void OnDrawGizmos()
        {
            var gridPositions = GetGridPositions();

            foreach (var position in gridPositions) 
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(GridToWorldPosition(position), new Vector3(GridCellSize*0.9f, 0.2f, GridCellSize*0.9f));
            }
        }
    }
}