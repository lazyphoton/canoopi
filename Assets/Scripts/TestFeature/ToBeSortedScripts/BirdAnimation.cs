using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour
{
    [SerializeField] float timeBetweenSwitch = 5; // 5 seconds is the length of the current animation
    [SerializeField] float birdDegreeSwitch = 30; // the angle the bird changes rotation
    private bool canRotate = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BirdSwitchSide());
    }


    private IEnumerator BirdSwitchSide() //prob need to add condition so bird doesn't flip around while in other states than eating
        // Currently not smooth, need to adjust
    {
        while (canRotate)
        {
            yield return new WaitForSeconds(timeBetweenSwitch); // wait for the length of the eating animation

            int positionValue = Random.Range(0, 2); // choose a value 
            int modification = 0;
            if (positionValue == 1) // one value makes a clockwise turn
            {
                modification = -1;
            }
            else if (positionValue == 0) // the other value makes an anti clockwise turn
            {
                modification = 1;
            }
            float currentYRotation = transform.eulerAngles.y;


            float targetRotation = currentYRotation + birdDegreeSwitch * modification;
            transform.eulerAngles = new Vector3(0, targetRotation, 0);

        }
    }
    
}
