using UnityEngine;

namespace GameCore
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotationPerSecond;

        private void Update()
        {
            transform.Rotate(rotationPerSecond * Time.deltaTime);
        }
    }
}