using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GameCore
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public Camera MainCamera { get { return _camera; } }

        private CinemachineVirtualCamera _previousVCam = null;

        private readonly int PrioLow = 10;
        private readonly int PrioHigh = 20;

        private Stack<CinemachineVirtualCamera> _cameraStack;

        private void Start()
        {
            // TODO: What about scene changes? Should that reset anything properly?
            _cameraStack = new Stack<CinemachineVirtualCamera>();
        }

        public void SetCameraAsPriority(CinemachineVirtualCamera virtualCamera)
        {
            _cameraStack.Clear();
            PushCamera(virtualCamera);
        }

        private void SetAsCurrentCamera(CinemachineVirtualCamera virtualCamera)
        {
            if (virtualCamera == null)
            {
                Log.Warning("Attempting to set a null virtual camera in view manager.");
                return;
            }

            if (_previousVCam != null)
            {
                _previousVCam.Priority = PrioLow;
            }

            virtualCamera.Priority = PrioHigh;
            _previousVCam = virtualCamera;
        }

        public void PushCamera(CinemachineVirtualCamera virtualCamera)
        {
            _cameraStack.Push(virtualCamera);
            SetAsCurrentCamera(virtualCamera);
        }

        public void PopCamera() 
        { 
            if(_cameraStack.Count == 0)
            {
                Log.Error("Trying to pop camera when there's none on the stack.");
                return;
            }

            _cameraStack.Pop();

            if (_cameraStack.Count == 0)
            {
                Log.Warning("Popped last camera off of stack.");
                return;
            }

            SetAsCurrentCamera(_cameraStack.Peek());
        }
    }
}