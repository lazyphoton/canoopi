using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class RequirementStepSequence
    {
        public event Action SequenceComplete;
        public event Action<IRequirementStep> StepStarted;

        private List<IRequirementStep> _steps;

        private int _currentStep;

        private RepeatingTimer _repeatingTimer;

        public IRequirementStep CurrentStep => _currentStep >= 0 && _currentStep < _steps.Count ? _steps[_currentStep] : null;
        public int CurrentStepIndex => _currentStep;

        public RequirementStepSequence()
        {
            _steps = new List<IRequirementStep>();
        }

        public void AddSteps(List<IRequirementStep> steps)
        {
            _steps.AddRange(steps);
        }

        public void AddStep(IRequirementStep step) 
        { 
            _steps.Add(step);
        }

        public void Start()
        {
            StartAtStep(0);
        }

        public void StartAtStep(int startStep)
        {
            if (_steps.Count == 0)
            {
                Log.Debug("Requirement sequence has no steps, completing immediately.");
                SequenceComplete?.Invoke();
                return;
            }

            if (startStep >= _steps.Count)
            {
                Log.Debug("Start step larger than number of steps, completing immediately.");
                SequenceComplete?.Invoke();
                return;
            }

            _currentStep = startStep;
            _steps[_currentStep].OnStepStart();
            StepStarted?.Invoke(_steps[_currentStep]);

            _repeatingTimer = World.GetService<TimeManager>().CreateAndStartRepeatingTimer(0.3f, CheckStep);
        }

        private void CheckStep()
        {
            var currentStep = _steps[_currentStep];

            if (currentStep.IsRequirementMet())
            {
                currentStep.OnStepComplete();
                _currentStep++;
                
                if(_currentStep == _steps.Count)
                {
                    // Sequence complete
                    SequenceComplete?.Invoke();
                    Stop();
                }
                else
                {
                    // Start next step
                    currentStep = _steps[_currentStep];
                    currentStep.OnStepStart();
                    StepStarted?.Invoke(_steps[_currentStep]);
                }
            }
        }

        public void Stop()
        {
            if (_repeatingTimer == null)
            {
                Log.Warning("Requirement step sequence's repeating timer already null.");
                return;
            }

            _repeatingTimer.Kill();
            _repeatingTimer = null;
        }
    }
}