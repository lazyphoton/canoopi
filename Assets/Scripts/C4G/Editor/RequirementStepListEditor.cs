using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c4g
{
    [CustomEditor(typeof(RequirementStepList))]
    public class RequirementStepListEditor : Editor
    {
        private string[] _stepNames = new string[] 
        { 
            "Wait For Seconds", 
            "Collect Item", 
            "Go To Object With Tag", 
            "Wait For Key Value", 
            "Wait For GameVariable" 
        };

        private Type[] _stepTypes = new Type[] 
        { 
            typeof(RSWaitSeconds), 
            typeof(RSCollectItem), 
            typeof(RSGoToObjectWithTag), 
            typeof(RSWaitForKeyValue), 
            typeof(RSWaitForGameVariable) 
        };

        private int _chosenStepIndex = 0;

        private string[] _eventNames = new string[] 
        { 
            "None", 
            "Show Info", 
            "Set Value",
            "Set GameVariable"
        };

        private Type[] _eventTypes = new Type[] 
        { 
            null, 
            typeof(RSEShowInfo), 
            typeof(RSESetKeyValue),
            typeof(RSESetGameVariable)
        };

        private int _chosenEventIndex = 0;

        private RequirementStepList _stepList;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _stepList = (RequirementStepList)target;

            EditorGUILayout.LabelField("Add a new requirement step of the chosen requirement and on complete type:");

            EditorGUILayout.BeginHorizontal();

            _chosenStepIndex = EditorGUILayout.Popup(_chosenStepIndex, _stepNames);
            _chosenEventIndex = EditorGUILayout.Popup(_chosenEventIndex, _eventNames);

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button($"Add Step: {_stepNames[_chosenStepIndex]} with on complete: {_eventNames[_chosenEventIndex]}"))
            {
                _stepList.AddStepByType(_stepTypes[_chosenStepIndex], _eventTypes[_chosenEventIndex]);
            }
        }
    }
}
