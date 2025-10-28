using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    // Testing the requirement step quest system
    public class QuestManager : MonoBehaviour
    {
        public event Action QuestComplete;
        public event Action<IRequirementStep> QuestStepStarted;

        private RequirementStepSequence _questSequence;

        public IRequirementStep CurrentStep => _questSequence?.CurrentStep;

        private PlayerInformationManager _playerInformationManager;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();

            _playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();

            if(_playerInformationManager.CurrentQuest != null)
            {
                SetQuest(_playerInformationManager.CurrentQuest);
            }
        }

        private void SetQuest(QuestState questState)
        {
            // TODO: What if another quest is already in progress?

            _playerInformationManager.CurrentQuest = questState;

            _questSequence = new RequirementStepSequence();
            _questSequence.AddSteps(questState.StepList.Steps);

            _questSequence.StepStarted += OnStepStarted;
            _questSequence.SequenceComplete += OnSequenceComplete;

            _questSequence.StartAtStep(questState.CurrentStepIndex);
        }

        public void StartQuest(RequirementStepList stepList)
        {
            var questState = new QuestState(stepList);
            SetQuest(questState);
        }

        private void OnDestroy()
        {
            if (_questSequence != null)
            {
                _questSequence.StepStarted -= OnStepStarted;
                _questSequence.SequenceComplete -= OnSequenceComplete;

                _questSequence.Stop();
            }
        }

        private void OnStepStarted(IRequirementStep step)
        {
            QuestStepStarted?.Invoke(step);
            _playerInformationManager.CurrentQuest.CurrentStepIndex = _questSequence.CurrentStepIndex;
        }

        private void OnSequenceComplete()
        {
            QuestComplete?.Invoke();
            _playerInformationManager.CurrentQuest = null;

            _questSequence.StepStarted -= OnStepStarted;
            _questSequence.SequenceComplete -= OnSequenceComplete;

            _questSequence = null;
        }

        public void CancelQuest()
        {
            _playerInformationManager.CurrentQuest = null;

            if(_questSequence != null)
            {
                _questSequence.StepStarted -= OnStepStarted;
                _questSequence.SequenceComplete -= OnSequenceComplete;

                _questSequence = null;
            }
        }
    }
}