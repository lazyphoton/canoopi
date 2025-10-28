using UnityEngine;

namespace c4g
{
    public class QuestState
    {
        public RequirementStepList StepList { get; private set; }
        public int CurrentStepIndex { get; set; }

        public QuestState(RequirementStepList stepList)
        {
            StepList = stepList;
            CurrentStepIndex = 0;
        }
    }
}