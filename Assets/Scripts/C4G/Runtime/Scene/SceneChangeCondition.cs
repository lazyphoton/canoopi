using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(fileName = "NewSceneChangeCondition", menuName = "C4G/SceneChangeCondition")]
    public class SceneChangeCondition : ScriptableObject
    {
        [SerializeField]
        private GameSceneDefinition _sceneDefinition;

        [SerializeField]
        private GCGameVariable _gameVariableCondition;

        public GameSceneDefinition SceneDefinition => _sceneDefinition;

        public GCGameVariable GameVariableCondition => _gameVariableCondition;
    }
}