using Cinemachine;
using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class UIFrame_MG_BundBranches : UIFrameMinigame
    {
        public const int STATE_STARTED = 5;
        public const int STATE_FINISHING = 10;
        public const int STATE_FINISHED = 15;

        [Header("Branch Placing")]
        [SerializeField]
        private DialogPartDefinition _dialogStart;

        [SerializeField]
        private DialogPartDefinition _dialogEnd;

        private BranchGrid _branchGrid;

        private GridBranch _holdingBranch = null;

        public override void OnPush(Dictionary<string, object> injectedInfo)
        {
            BaseMinigameSetup();

            _interactionManager.PointerDownRaycastHit += OnPointerDown;
            _interactionManager.PointerClickRaycastHit += OnPointerClick;

            var levelLayoutObject = GameObject.Find("LevelLayout_MG_BranchPlacing");

            _branchGrid = levelLayoutObject.GetComponentInChildren<BranchGrid>();

            var mainVirtualCam = levelLayoutObject.transform.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            _viewManager.SetCameraAsPriority(mainVirtualCam);

            // Do start dialog first, wait for other panels to load
            _timeManager.DoAfterShortDelay(() => { UiManager.PushUIDialog(_dialogStart, null); });
        }

        public override void OnPop(Dictionary<string, object> injectedInfo)
        {
            _interactionManager.PointerDownRaycastHit -= OnPointerDown;
            _interactionManager.PointerClickRaycastHit -= OnPointerClick;

            BaseMinigameTeardown();
        }

        public override void OnFallbackFocus(Dictionary<string, object> injectedInfo)
        {

        }

        private void Update()
        {
            var currentMingameState = GetMinigameState();

            switch (currentMingameState)
            {
                case 0:
                    // In starting dialog
                    break;
                case STATE_STARTED:
                    // Doing the game
                    UpdatePlacing();
                    break;
                case STATE_FINISHING:
                    // In ending dialog
                    break;
                case STATE_FINISHED:
                    // Done the minigame
                    OnMinigameComplete();
                    break;
            }
        }

        private void UpdatePlacing()
        {
            if(_holdingBranch != null)
            {
                // Update position of current branch

                var ray = _interactionManager.GetCurentPointerRay();
                var layer = 1 << LayerMask.NameToLayer("Floor");

                if (Physics.Raycast(ray, out var hitInfo, 100f, layer))
                {
                    _holdingBranch.SetPosition(hitInfo.point);
                }
            }

            if (_branchGrid.IsGridComplete())
            {
                SetMinigameState(STATE_FINISHING);
                UiManager.PushUIDialog(_dialogEnd, null);
            }
        }

        private void PickUpBranch(GridBranch branch)
        {
            _holdingBranch = branch;
            _holdingBranch.SetHolding(true);
            _branchGrid.UnoccupyBranch(branch);
        }

        private void DropBranch()
        {
            if(_holdingBranch == null)
            {
                return;
            }

            _branchGrid.TryPlaceBranch(_holdingBranch);
            _holdingBranch.SetHolding(false);

            _holdingBranch = null;
        }

        private void OnPointerDown(RaycastHit hitInfo)
        {
            var gridBranch = hitInfo.collider.GetComponentInParent<GridBranch>();

            if(gridBranch != null)
            {
                PickUpBranch(gridBranch);
            }
        }

        private void OnPointerClick(RaycastHit hitInfo)
        {
            // TODO: What if let go over ui (aka not catchable)

            DropBranch();
        }
    }
}