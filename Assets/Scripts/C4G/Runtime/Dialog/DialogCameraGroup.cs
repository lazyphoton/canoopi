using Cinemachine;
using GameCore;
using UnityEngine;

namespace c4g
{
    public class DialogCameraGroup : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        [SerializeField]
        private Transform[] _cameraTransforms;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        public void SetDefaultCameraTransform()
        {
            SetCameraTransformIndex(0);
        }

        public void SetCameraTransformIndex(int index)
        {
            Log.Debug($"Setting dialog camera to transform index {index}");

            if(_cameraTransforms.Length == 0 || _cameraTransforms[0] == null)
            {
                Log.Error("Dialog camera group has no default camera transform (0 index in transforms array).");
                return;
            }

            var transformToUse = _cameraTransforms[0];

            if(index > 0)
            {
                if (index < _cameraTransforms.Length && _cameraTransforms[index] != null)
                {
                    transformToUse = _cameraTransforms[index];
                }
                else
                {
                    Log.Error($"Dialog camera group has no transform at index {index}");
                }
            }

            _virtualCamera.transform.SetPositionAndRotation(transformToUse.transform.position, transformToUse.transform.rotation);
        }

        private void OnDrawGizmos()
        {
            if(_virtualCamera == null)
            {
                return;
            }

            foreach (var camTransform in _cameraTransforms) 
            { 
                if(camTransform == null)
                {
                    continue;
                }

                Gizmos.color = new Color(0f, 1f, 0.9f, 0.3f);

                var previousMatrix = Gizmos.matrix;
                Gizmos.matrix = camTransform.localToWorldMatrix;
                Gizmos.DrawFrustum(Vector3.zero, _virtualCamera.m_Lens.FieldOfView, 30f, 0.1f, 16f/9f);
                Gizmos.matrix = previousMatrix;

                Gizmos.color = new Color(0f, 1f, 0.9f, 0.45f);
                Gizmos.DrawLine(camTransform.transform.position, camTransform.transform.position + (camTransform.forward * 30f));
            }
        }
    }
}