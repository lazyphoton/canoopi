using GameCore;
using UnityEngine;

namespace c4g
{
    public class TestQuestManagement : MonoBehaviour
    {
        [SerializeField]
        private RequirementStepList _testRequirementList = null;

        [SerializeField]
        private bool _cancelCurrentQuest = false;

        [SerializeField]
        private bool _emptyInventory = false;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var awaiter = World.GetService<Awaiter>();

            var questManager = await awaiter.AwaitServiceExistsAsync<QuestManager>();

            if (_cancelCurrentQuest)
            {
                questManager.CancelQuest();
            }

            var playerInformationManager = await awaiter.AwaitServiceExistsAsync<PlayerInformationManager>();
            await awaiter.AwaitConditionAsync(() => playerInformationManager.HasPlayerChosenVisual);

            if (_emptyInventory) 
            {
                playerInformationManager.CurrentPlayerInventory.Empty();
            }

            if(_testRequirementList != null && playerInformationManager.CurrentQuest == null)
            {
                questManager.StartQuest(_testRequirementList);
            }
        }
    }
}