using UnityEngine;

namespace GameGore
{
    public static class GizmosExtensions
    {
        private static Color _oldGizmoColor;
        private static Matrix4x4 _oldGizmoMatrix;

        private static void SaveGizmoState()
        {
            _oldGizmoColor = Gizmos.color;
            _oldGizmoMatrix = Gizmos.matrix;
        }

        private static void RestoreGizmoState()
        {
            Gizmos.matrix = _oldGizmoMatrix;
            Gizmos.color = _oldGizmoColor;
        }

        public static void DrawDisk(Vector3 position, float radius, Color color)
        {
            SaveGizmoState();
            
            Gizmos.color = color;
            Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(1f, 0.05f, 1f));
            Gizmos.DrawSphere(Vector3.zero, radius);

            RestoreGizmoState();
        }
    }
}