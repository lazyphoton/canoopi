using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(fileName = "NewGameSceneDefinition", menuName = "C4G/GameSceneDefinition")]
    public class GameSceneDefinition : ScriptableObject
    {
        [SerializeField]
        private string _sceneName = "";


        public string SceneName => _sceneName;
    }
}