using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CountdownTimer : MonoBehaviour
    {
        private Action<int> _onSecondsRemainingChanged;

        private float _timeRemaining;
        
        private bool _running = false;
        private bool _initialized = false;

        public void Initialize(float seconds, Action<int> onSecondsRemainingChanged)
        {
            if (_initialized)
            {
                Log.Warning("Attempting to initialize an already initialized timer.");
                return;
            }

            _initialized = true;

            _onSecondsRemainingChanged = onSecondsRemainingChanged;
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
                return;

            var newTime = _timeRemaining - Time.deltaTime;

            var flooredTimeRemaining = Mathf.FloorToInt(_timeRemaining);
            var flooredNewTime = Mathf.Max(Mathf.FloorToInt(newTime), 0);

            _timeRemaining = newTime;

            if (flooredNewTime < flooredTimeRemaining)
            {
                _onSecondsRemainingChanged?.Invoke(flooredNewTime);
            }

            if(flooredNewTime == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}