using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSWaitSeconds : ARequirementStep
    {
        public override string Description => $"Wait {_duration} seconds.";

        [Header("Wait For Seconds")]
        [SerializeField]
        private float _duration;

        private bool _timeReached;

        public RSWaitSeconds() { }

        public RSWaitSeconds(float duration)
        {
            _duration = duration;
        }

        public override void OnStepStart()
        {
            _timeReached = false;
            World.GetService<TimeManager>().DoAfterSeconds(() => { _timeReached = true; }, _duration);
        }

        public override bool IsRequirementMet()
        {
            return _timeReached;
        }
    }
}