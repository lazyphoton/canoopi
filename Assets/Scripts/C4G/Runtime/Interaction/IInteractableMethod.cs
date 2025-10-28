using UnityEngine;

namespace c4g
{
    public interface IInteractableMethod
    {
        public string Text { get; }
        public Sprite Icon { get; }

        public void Interact();
    }
}