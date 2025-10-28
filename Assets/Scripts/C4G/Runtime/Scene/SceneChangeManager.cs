using GameCore;
using UnityEngine;

namespace c4g
{
    public class SceneChangeManager
    {
        private SceneChangeCondition[] _sceneChangeConditions;

        public SceneChangeManager() 
        {
            _sceneChangeConditions = Resources.LoadAll<SceneChangeCondition>("SceneChange");

            World.GetService<GameVariableManager>().GameVariableChanged += OnGameVariableChanged;
        }

        private void OnGameVariableChanged(GameVariableDefinition gameVariableDefintion)
        {
            foreach (var sceneCondition in _sceneChangeConditions) 
            { 
                if(gameVariableDefintion != sceneCondition.GameVariableCondition.GameVariableDefinition)
                {
                    continue;
                }

                // TODO: Problem if the variable is changed again to the same value?
                // TODO: Problem if the variable is already the right value but never changes?

                if (sceneCondition.GameVariableCondition.IsConditionMet())
                {
                    Unregister();
                    ChangeScene(sceneCondition.SceneDefinition);
                    return;
                }
            }
        }

        private void Unregister()
        {
            World.GetService<GameVariableManager>().GameVariableChanged -= OnGameVariableChanged;
        }

        private void ChangeScene(GameSceneDefinition scene)
        {
            World.GetService<ISceneManager>().LoadSceneSingleAfterTransitionAsync(scene);
        }
    }
}