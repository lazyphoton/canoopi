using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "UIFD", menuName = "GAME CORE/UIFrameDefinition")]
    public class UIFrameDefinition : ScriptableObject
    {
        [SerializeField]
        private GameObject _uiFramePrefab;

        public GameObject UIFramePrefab => _uiFramePrefab;
    }
}