using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore
{
    public class Awaiter : MonoBehaviour
    {
        public async Task AwaitConditionAsync(Func<bool> conditionFunction)
        {
            while (conditionFunction?.Invoke() == false)
            {
                // A little weird, but works for now?
                await Task.Yield();
            }
        }

        public async Task<T> AwaitServiceExistsAsync<T>() where T : class
        {
            await AwaitConditionAsync(() => { return World.GetService<T>() != null; });
            return World.GetService<T>();
        }
    }
}