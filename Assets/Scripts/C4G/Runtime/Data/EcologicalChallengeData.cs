using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(fileName = "NewChallenge", menuName = "C4G/Eco Challenges")]
    public class EcologicalChallengeData : ScriptableObject
    {
        public string buttonLabel;
        public string title;
        [TextArea]
        public string description;
        public Material overlayMaterial;
    }
}