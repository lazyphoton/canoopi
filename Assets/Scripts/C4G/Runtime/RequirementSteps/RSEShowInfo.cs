using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSEShowInfo : IRequirementStepEvent
    {
        [Header("Show Info Text")]
        [SerializeField]
        [TextArea(5, 20)]
        private string _infoText;

        public void Trigger()
        {
            World.GetService<UIManager>().PushUIInfo(_infoText);
        }
    }
}