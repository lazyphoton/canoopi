using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace GameCore
{
    public class TimeManager : MonoBehaviour
    {
        public void DoAfterSeconds(Action action, float delay)
        {
            DOVirtual.Float(0f, 1f, delay, (float val) => { }).OnComplete(() => { action.Invoke(); });
        }

        public CountdownTimer CreateAndStartCountdownTimer(float seconds, Action<int> onSecondsRemainingChanged)
        {
            var timerObject = new GameObject("Countdown Timer Object");
            timerObject.transform.SetParent(transform);

            var timer = timerObject.AddComponent<CountdownTimer>();
            timer.Initialize(seconds, onSecondsRemainingChanged);
            return timer;
        }

        public RepeatingTimer CreateAndStartRepeatingTimer(float seconds, Action onTimeReached)
        {
            var timerObject = new GameObject("Repeating Timer Object");
            timerObject.transform.SetParent(transform);

            var timer = timerObject.AddComponent<RepeatingTimer>();
            timer.Initialize(seconds, onTimeReached);
            return timer;
        }
    }
}