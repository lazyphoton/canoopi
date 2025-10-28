using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c4g
{
    public abstract class ARequirementStep : IRequirementStep
    {
        public abstract string Description { get; }

        [SerializeReference]
        private IRequirementStepEvent _onCompleteEvent;

        public abstract bool IsRequirementMet();

        public abstract void OnStepStart();

        public virtual void OnStepComplete()
        {
            _onCompleteEvent?.Trigger();
        }

        public void SetOnCompleteEvent(IRequirementStepEvent onCompleteEvent)
        {
            _onCompleteEvent = onCompleteEvent;
        }
    }
}