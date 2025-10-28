using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    // Should be part of player - Keeps track of player's mission progress

    [SerializeField] List<GameObject> totalMissionObjectives = new List<GameObject>(); // Add objects to the list in the editor (or automate process to avoid mistakes)

    [SerializeField] List<GameObject> currentMissionObjectives = new List<GameObject>(); // Objects of the current mission will be added here

    [SerializeField] List<GameObject> newMissionObjective = new List<GameObject>(); // Temp code, need logic review




    void Start()
    {
        
    }

    public void ObjectiveSearchedObjectFound(GameObject foundObject)
    {
        currentMissionObjectives.Add(foundObject);
        if (totalMissionObjectives.Count == currentMissionObjectives.Count) // Trying to save some performance - LCC
        {
            CheckObjectiveComplete();
            //ChangeObjectiveList(); // TEMP - May require some logic change
        }
    }

    private void CheckObjectiveComplete()
    {
        bool areEqual = new HashSet<GameObject>(totalMissionObjectives).SetEquals(currentMissionObjectives);  // Method to check if both lists have the same unique elements (each Game Object should be unique)

        if (areEqual == true)
        {
            ObjectiveComplete();
        }
    }

    private void ObjectiveComplete()
    {
        Debug.Log("Woohoo, the objective is complete!");
    }

    private void ChangeObjectiveList()
    {
        totalMissionObjectives.Clear(); // Clears previous objectives
        totalMissionObjectives.AddRange(newMissionObjective); // Adds new objectives
    }
}
