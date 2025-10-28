using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace c4g
{
    public class Navigator : MonoBehaviour
    {
        public event Action MovementStarted;
        public event Action MovementStopped;

        public float Speed => _navMeshAgent == null ? 0f : _navMeshAgent.velocity.magnitude;

        private NavMeshAgent _navMeshAgent;

        private Action _onReachDestinationCallback;

        private float _previousRemainingDistance;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_navMeshAgent.pathPending)
            {
                //Log.Debug("Skipping navigator update because agent path pending.");
                return;
            }
            
            var remainingDistance = _navMeshAgent.remainingDistance;

            //Log.Debug($"Remaining distance: {remainingDistance}");

            if(remainingDistance == 0 && _previousRemainingDistance > 0)
            {
                // Movement has stopped and the path has been completed (probably)
                // This does not account for potentially strange situations where the path is
                // actually not completed and the character stops because of an error

                MovementStopped?.Invoke();
                InvokeDestinationCallback();
            }

            if(remainingDistance > _previousRemainingDistance)
            {
                // Movement has been started
                if(_navMeshAgent.velocity.magnitude > 0.01f)
                {
                    // Started while still moving?
                }
                else
                {
                    MovementStarted?.Invoke();
                }
            }

            _previousRemainingDistance = remainingDistance;
        }

        public void SetTargetDestination(Vector3 worldPositionTarget, Action onReachDestinationCallback = null)
        {
            _navMeshAgent.destination = worldPositionTarget;
            _onReachDestinationCallback = onReachDestinationCallback;
            _previousRemainingDistance = 0f;

            var mag = (worldPositionTarget - transform.position).magnitude;

            //Log.Debug($"World pos target: {worldPositionTarget}, current transform pos: {transform.position}, dist: {mag}");

            // Bit of a hack to make it so that it completes automatically if we're considered close enough.
            // This will probably have to be revisited, but navmesh stuff is really finnixky and weird.
            if (mag < 2f)
            {
                // Close enough to consider it complete by default
                InvokeDestinationCallback();
            }
        }

        private void InvokeDestinationCallback()
        {
            _onReachDestinationCallback?.Invoke();

            // Make sure the path doesn't try to get updated after
            // performing a callback that removes the blocker for example
            StopNavigation();
        }

        public void StopNavigation()
        {
            //Log.Debug("Navigation stopped");

            _navMeshAgent.destination = transform.position;
            _onReachDestinationCallback = null;
        }

        public void LookAtTarget(Vector3 worldPositionTarget)
        {
            transform.LookAt(worldPositionTarget, Vector3.up);
        }
    }
}