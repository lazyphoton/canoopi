using GameCore;
using UnityEngine;

namespace c4g
{
    public static class GameVariableManagerExtensions
    {
        public static bool TryGetBool(this GameVariableManager gameVariableManager, string key, out bool value) 
        {
            return gameVariableManager.TryGetValue<bool>(key, out value);
        }

        public static bool TryGetInt(this GameVariableManager gameVariableManager, string key, out int value)
        {
            return gameVariableManager.TryGetValue<int>(key, out value);
        }

        public static bool TryGetString(this GameVariableManager gameVariableManager, string key, out string value)
        {
            return gameVariableManager.TryGetValue<string>(key, out value);
        }

        public static void SetValue(this GameVariableManager gameVariableManager, GameVariableDefinition gameVariableDefinition, string value)
        {
            switch (gameVariableDefinition.VariableType)
            {
                case GameVariableType.Bool:
                    gameVariableManager.SetBool(gameVariableDefinition.VariableId, value);
                    return;
                case GameVariableType.Int:
                    gameVariableManager.SetInt(gameVariableDefinition.VariableId, value);
                    return;
                case GameVariableType.String:
                    gameVariableManager.SetString(gameVariableDefinition.VariableId, value);
                    return;
            }

            Log.Error($"No requirement check for GameVariable type: {gameVariableDefinition.VariableType}");
        }

        public static void SetBool(this GameVariableManager gameVariableManager, string key, string value)
        {
            if (bool.TryParse(value, out var result))
            {
                gameVariableManager.SetBool(key, result);
                return;
            }

            Log.Error($"Could not parse \"{value}\" into bool.");
        }

        public static void SetBool(this GameVariableManager gameVariableManager, string key, bool value)
        {
            gameVariableManager.SetValue<bool>(key, value);
        }

        public static void SetInt(this GameVariableManager gameVariableManager, string key, string value)
        {
            if (int.TryParse(value, out var result))
            {
                gameVariableManager.SetInt(key, result);
                return;
            }

            Log.Error($"Could not parse \"{value}\" into int.");
        }

        public static void SetInt(this GameVariableManager gameVariableManager, string key, int value)
        {
            gameVariableManager.SetValue<int>(key, value);
        }

        public static void SetString(this GameVariableManager gameVariableManager, string key, string value)
        {
            gameVariableManager.SetValue<string>(key, value);
        }
    }
}