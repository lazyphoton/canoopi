using UnityEngine;
using GameCore;
using System.Collections.Generic;

namespace c4g
{
    public static class UIManagerExtensions
    {
        public const string UIKEY_InjectedSceneObjects = "UIFrameInjectedSceneObjects";

        private static UIFrameDefinitionHolder _uiFrameDefinitionHolder;

        public static void SetUIFrameDefinitionHolderInstance(UIFrameDefinitionHolder uiFrameDefinitionHolder)
        {
            _uiFrameDefinitionHolder = uiFrameDefinitionHolder;
        }

        public static void PushUILoadScene(this UIManager uiManager, GameSceneDefinition scene)
        {
            uiManager.PushUI(
                _uiFrameDefinitionHolder.LoadSceneFrameDefinition,
                new Dictionary<string, object>() { { UIFrameLoadScene.UIKEY_LoadScene, scene } });
        }

        public static void PushUIDialog(
            this UIManager uiManager, 
            DialogPartDefinition startingDialogPart,
            GameObject dialogCameraGroupObject)
        {
            uiManager.PushUI(
                _uiFrameDefinitionHolder.DialogFrameDefinition,
                new Dictionary<string, object>() 
                { 
                    { UIFrameDialog.UIKEY_StartingDialogPart, startingDialogPart },
                    { UIFrameDialog.UIKEY_DialogCameraGroupObject, dialogCameraGroupObject }
                });
        }

        public static void PushUIInfo(this UIManager uiManager, string infoText)
        {
            uiManager.PushUI(
                _uiFrameDefinitionHolder.InfoTextFrameDefinition,
                new Dictionary<string, object>() { { UIFrameInfo.UIKEY_InfoText, infoText } });
        }

        public static void PushUICharacterSelect(this UIManager uIManager)
        {
            uIManager.PushUI(_uiFrameDefinitionHolder.CharacterSelectFrameDefinition);
        }

        public static void PushUIInteractionChoice(this UIManager uiManager, List<IInteractableMethod> interactableMethods, Vector3 hitPosition)
        {
            uiManager.PushUI(
                _uiFrameDefinitionHolder.InteractionChoiceFrameDefinition,
                new Dictionary<string, object>() 
                { 
                    { UIFrameInteractionChoice.UIKEY_InteractableMethods, interactableMethods },
                    { UIFrameInteractionChoice.UIKEY_HitPosition, hitPosition }
                });
        }

        public static void PushUIShowUI(this UIManager uiManager, UIFrameDefinition uiFrameDefinition, UnityEngine.Object[] sceneObjects)
        {
            uiManager.PushUI(
                uiFrameDefinition,
                new Dictionary<string, object>() { { UIManagerExtensions.UIKEY_InjectedSceneObjects, sceneObjects } });
        }
    }
}