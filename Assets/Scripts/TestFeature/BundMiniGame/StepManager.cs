using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public int currentStepBundMiniGame = 0;
    [Header("Guiding Light")]
    [SerializeField] GameObject vfxGuidingPlayer;
    [SerializeField] Transform vfxPositionOne;
    [SerializeField] Transform vfxPositionTwo;

    //[Header("On Trigger Enter Objectives")]
    //[SerializeField] GameObject FirstCheckpoint;


    private void Start()
    {
        StepCompleted(0);
    }

    public void StepCompleted(int stepCompleted)
    {
        currentStepBundMiniGame = stepCompleted; // Update current step to the new step
        currentStepBundMiniGame += 1; // Update to move on to the next step

        Debug.Log("Current step is " + currentStepBundMiniGame);

        switch (currentStepBundMiniGame) // There's definitely a better way to do this but here we go
        {
            case 1:
                StepOneFunction();

                break;

            case 2:
                StepTwoFunction();
                break;

        }
    }
    private void TranformVfxPosition(GameObject objectToMove, Transform newPosition)
    {
        objectToMove.transform.position = newPosition.transform.position;
    }

    private void SwitchVfxState(GameObject vfxToSwitch, bool isActivated)
    {
        vfxToSwitch.SetActive(isActivated);
    }
    private void StepOneFunction()
    {
        TranformVfxPosition(vfxGuidingPlayer, vfxPositionOne);
        SwitchVfxState(vfxGuidingPlayer, true);
        // Update Ui Text 
    }

    private void StepTwoFunction()
    {
        TranformVfxPosition(vfxGuidingPlayer, vfxPositionTwo);
        // Update Ui Text 
    }

    public bool ShouldRopeBePickedUp()
    {
        switch (currentStepBundMiniGame) // Used by the rope tip to know if the player should be able to move it again
        {
            case 1:
                return true;


            case 2:
                return false;
                

        }

        return true; // maybe set the default to false
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StepCompleted(currentStepBundMiniGame);
            Debug.Log("Step is completed");
        }
    }
}
