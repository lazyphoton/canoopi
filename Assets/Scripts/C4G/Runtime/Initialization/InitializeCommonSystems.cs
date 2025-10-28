using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class InitializeCommonSystems : MonoBehaviour
    {
        private void Start()
        {
            World.Services.SetService(typeof(IResourceProvider), new UnityFolderResourceProvider(), true);
            World.Services.SetService(typeof(UIManager), GetComponent<UIManager>(), false);
            World.Services.SetService(typeof(ViewManager), GetComponent<ViewManager>(), false);
            World.Services.SetService(typeof(InputManager), GetComponent<InputManager>(), false);
            World.Services.SetService(typeof(GameVariableManager), new GameVariableManager(), true);
            World.Services.SetService(typeof(PlayerInformationManager), new PlayerInformationManager(), true);
            World.Services.SetService(typeof(InteractionManager), new InteractionManager(), false);
            World.Services.CreateComponentService(typeof(QuestManager), typeof(QuestManager), false);
            World.Services.SetService(typeof(SceneChangeManager), new SceneChangeManager(), false);
        }
    }
}