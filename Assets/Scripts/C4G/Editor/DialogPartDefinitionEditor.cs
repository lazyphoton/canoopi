using CodiceApp.EventTracking.Plastic;
using System;
using UnityEditor;
using UnityEngine;

namespace c4g
{
    [CustomEditor(typeof(DialogPartDefinition))]
    [CanEditMultipleObjects]
    public class DialogPartDefinitionEditor : Editor
    {
        private DialogPartDefinition _dialogPart;

        private string[] _conditionNames = new string[]
        {
            "GameVariable",
            "Collect Item"
        };

        private Type[] _conditionTypes = new Type[]
        {
            typeof(GCGameVariable),
            typeof(GCCollectItem)
        };

        private int _chosenConditionIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _dialogPart = (DialogPartDefinition)target;

            EditorGUILayout.LabelField("Add a new conditional next dialog part of the chosen condition type:");

            EditorGUILayout.BeginHorizontal();

            _chosenConditionIndex = EditorGUILayout.Popup(_chosenConditionIndex, _conditionNames);

            if (GUILayout.Button($"Add Condition"))
            {
                _dialogPart.AddConditionlNextDialogPartByType(_conditionTypes[_chosenConditionIndex]);
            }

            EditorGUILayout.EndHorizontal();

            if (string.IsNullOrWhiteSpace(_dialogPart.DialogText))
            {
                EditorGUILayout.HelpBox("Dialog text is empty, so this dialog part will be treated only as a condition check", MessageType.Info);
            }
        }
    }
}