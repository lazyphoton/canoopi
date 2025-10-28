using GameCore;

namespace c4g
{
    public static class ISceneManagerExtensions
    {
        public static void LoadSceneSingleAfterTransitionAsync(this ISceneManager sceneManager, GameSceneDefinition sceneDefinition)
        {
            // Change scene after doing the hide transition
            World.GetService<UIManager>().DoSceneTransitionHide(() => { sceneManager.LoadSceneSingleAsync(sceneDefinition.SceneName); });
        }
    }
}