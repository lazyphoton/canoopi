using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTip : MonoBehaviour
{
    [SerializeField] private GameObject anchorPoint;
    // Start is called before the first frame update


    [SerializeField] StepManager _stepManagerScript;
    private void Start()
    {
       // transform.position = anchorPoint.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            if (_stepManagerScript.ShouldRopeBePickedUp() == true) // calls a function to check if rope should be able to be moved at the current step
            {
            anchorPoint.GetComponent<LineRendererRope>().RopePickedUp(true);

            }
        }
    }


}
