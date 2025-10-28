using UnityEngine;

namespace c4g
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private GameSceneDefinition _scene;

        public GameSceneDefinition Scene => _scene;
    }
}