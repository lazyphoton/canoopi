using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public interface IRequirementStepEvent
    {
        public void Trigger();
    }
}