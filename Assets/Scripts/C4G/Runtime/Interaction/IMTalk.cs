using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c4g
{
    public class IMTalk : InteractableMethod
    {
        public override string Text => "TALK";
        public override Sprite Icon => World.GetService<IResourceProvider>().GetResource<Sprite>(Path.Combine("UI", "IM_Talk"));

        [Header("Injected Info")]
        [SerializeField]
        private DialogPartDefinition _startingDialogPart;

        [SerializeField]
        private GameObject _dialogCamGroupObject;

        [Header("Auto Start Dialog")]
        [SerializeField]
        private GCGameVariable[] _autoStartConditions = new GCGameVariable[0];

        private void Start()
        {
            Log.Debug("IMTalk start");

            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();
            var uiManager = await awaiter.AwaitServiceExistsAsync<UIManager>();

            foreach (var condition in _autoStartConditions)
            {
                Log.Debug($"Condition {condition.GameVariableDefinition.name} is {condition.TargetValue} : {condition.IsConditionMet()}");

                if (condition.IsConditionMet())
                {
                    // Watch out for timing issues, if not careful, this could happen before hte scene initializer
                    // sets the main UI frame for the scene
                    World.GetService<TimeManager>().DoAfterSeconds(() =>
                    {
                        uiManager.PushUIDialog(_startingDialogPart, _dialogCamGroupObject);
                    }, 0.25f);
                    break;
                }
            }
        }

        public override void Interact()
        {
            base.Interact();
            GoPlayerToTarget(OnGoComplete);
        }

        private void OnGoComplete()
        {
            LookPlayerAtTarget();
            World.GetService<UIManager>().PushUIDialog(_startingDialogPart, _dialogCamGroupObject);
        }
    }
}