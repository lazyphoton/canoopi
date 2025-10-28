using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class RSCollectItem : ARequirementStep
    {
        public override string Description
        {
            get
            {
                if (_collectItemConditions == null || _collectItemConditions.Count == 0)
                {
                    return "Collect - null";
                }

                var description = "Collect <sprite=7>:";

                foreach (var condition in _collectItemConditions)
                {
                    var amount = condition.Amount;
                    var iconIndex = condition.ItemDefinition.IconIndex;

                    var amountText = amount > 1 ? condition.Amount.ToString() : "";
                    var iconText = iconIndex >= 0 ? $"<sprite={iconIndex}>" : "";

                    description += $"\n - {amountText} <b>{condition.ItemDefinition.ItemName}</b> {iconText}";
                }

                return description;
            }
        }

        [SerializeField]
        private List<GCCollectItem> _collectItemConditions;

        public override void OnStepStart()
        {
            
        }

        public override bool IsRequirementMet()
        {
            if (_collectItemConditions == null || _collectItemConditions.Count == 0)
            {
                Log.Error($"Null or empty condition in requirement step: {Description}");
                return false;
            }

            var conditionMet = true;

            foreach(var condition in _collectItemConditions)
            {
                if (!condition.IsConditionMet())
                {
                    conditionMet = false;
                    break;
                }
            }

            return conditionMet;
        }
    }
}