using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class RepeatingTimer : MonoBehaviour
    {
        private Action _onTimeReached;

        private float _timerDuration;
        private float _timeRemaining;

        private bool _running = false;
        private bool _initialized = false;

        public void Initialize(float seconds, Action onTimeReached)
        {
            if (_initialized)
            {
                Log.Warning("Attempting to initialize an already initialized timer.");
                return;
            }

            _initialized = true;

            _onTimeReached = onTimeReached;
            _timerDuration = seconds;
            _timeRemaining = seconds;
            _running = true;
        }

        public void Pause()
        {
            _running = false;
        }

        public void Resume()
        {
            _running = true;
        }

        public void Kill()
        {
            _running = false;
            Destroy(gameObject);
        }

        private void Update()
        {
            if (!_running)
            {
                return;
            }

            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining < 0) 
            { 
                _onTimeReached?.Invoke();
                _timeRemaining += _timerDuration;
            }
        }
    }
}