using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public static class TimeManagerExtensions
    {
        public static void DoAfterShortDelay(this TimeManager timeManager, Action action)
        {
            timeManager.DoAfterSeconds(action, 0.05f);
        }

        public static void DoAfterHalfSecond(this TimeManager timeManager, Action action)
        {
            timeManager.DoAfterSeconds(action, 0.5f);
        }
    }
}