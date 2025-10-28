using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(menuName = "C4G/Requirement Step List")]
    public class RequirementStepList : ScriptableObject
    {
        public List<IRequirementStep> Steps => _steps;

        [SerializeReference]
        private List<IRequirementStep> _steps = new List<IRequirementStep>();

        public void AddStepByType(Type stepType, Type eventType)
        {
            var obj = (IRequirementStep)Activator.CreateInstance(stepType);
            _steps.Add(obj);

            if (obj is ARequirementStep aStep && eventType != null)
            {
                aStep.SetOnCompleteEvent((IRequirementStepEvent)Activator.CreateInstance(eventType));
            }
        }
    }
}