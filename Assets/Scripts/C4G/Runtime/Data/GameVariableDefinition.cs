using GameCore;
using System;
using UnityEngine;

namespace c4g
{
    [CreateAssetMenu(fileName = "NewGameVariableDefinition", menuName = "C4G/GameVariableDefinition")]
    public class GameVariableDefinition : ScriptableObject
    {
        [SerializeField]
        private string _variableId = "";
        
        [SerializeField]
        private GameVariableType _variableType = GameVariableType.Bool;

        public string VariableId => _variableId;

        public GameVariableType VariableType => _variableType;

        public Type RealVariableType
        {
            get
            {
                switch (VariableType) 
                { 
                    case GameVariableType.Bool: return typeof(bool);
                    case GameVariableType.Int: return typeof(int);
                    case GameVariableType.String: return typeof(string);
                }

                Log.Error($"Cannot get real type for unrecognized GameVariable type: {VariableType}");
                return typeof(object);
            }
        }
    }

    public enum GameVariableType
    {
        Bool,
        Int,
        String
    }
}