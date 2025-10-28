using GameCore;
using GameGore;
using System;
using UnityEngine;

namespace c4g
{
    public abstract class InteractableMethod : MonoBehaviour, IInteractableMethod
    {
        [SerializeField]
        private Vector3 InteractionPositionOffset = Vector3.zero;

        [SerializeField]
        private Vector3 LookPositionOffset = Vector3.zero;

        public abstract string Text { get; }
        public abstract Sprite Icon { get; }

        public Vector3 InteractionPosition => transform.position + transform.TransformVector(InteractionPositionOffset);
        public Vector3 LookPosition => transform.position + transform.TransformVector(LookPositionOffset);

        private PlayerInformationManager _playerInformationManager;
        protected PlayerInformationManager PlayerInformationManager => _playerInformationManager;

        public virtual void Interact()
        {
            _playerInformationManager = World.GetService<PlayerInformationManager>();
        }

        protected void GoPlayerToTarget(Action onGoComplete)
        {
            _playerInformationManager.CurrentPlayer.Navigator.SetTargetDestination(InteractionPosition, onGoComplete);
        }

        protected void LookPlayerAtTarget()
        {
            _playerInformationManager.CurrentPlayer.Navigator.LookAtTarget(LookPosition);
        }

        private void OnDrawGizmos()
        {
            GizmosExtensions.DrawDisk(InteractionPosition, 0.4f, Color.cyan);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(LookPosition, 0.25f);
            Gizmos.DrawLine(InteractionPosition, LookPosition);
        }
    }
}