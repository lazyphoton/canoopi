using GameGore;
using UnityEngine;

namespace c4g
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The priority of this spawn point, higher number = higher priority.")]
        private int _priority = 0;

        [Header("Conditional Priority")]
        [SerializeField]
        private GCGameVariable[] _priorityConditions = new GCGameVariable[0];

        [SerializeField]
        private int _conditionalPriority = 10;

        public int Priority
        {
            get
            {
                foreach (var condition in _priorityConditions) 
                {
                    if (condition.IsConditionMet())
                    {
                        return _conditionalPriority;
                    }
                }

                return _priority;
            }
        }

        public Vector3 SpawnPosition => gameObject.transform.position;

        public float SpawnRotation => gameObject.transform.eulerAngles.y;

        private void OnDrawGizmos()
        {
            GizmosExtensions.DrawDisk(SpawnPosition, 0.5f, Color.green);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(SpawnPosition, SpawnPosition + transform.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(SpawnPosition, SpawnPosition + transform.forward);

            Gizmos.DrawIcon(SpawnPosition + transform.up, "PlayerSpawnIcon");
        }
    }
}