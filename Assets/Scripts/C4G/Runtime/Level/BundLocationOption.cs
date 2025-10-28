using UnityEngine;

namespace c4g
{
    public enum BundLocationTag
    {
        None,
        GroundRocky,
        GroundSandy,
        GroundSoil,
        SlopeSteep,
        SlopeShallow
    }

    public class BundLocationOption : MonoBehaviour
    {
        [SerializeField]
        private BundLocationTag _bundLocationTag = BundLocationTag.None;

        public BundLocationTag BundLocationTag => _bundLocationTag;
    }
}