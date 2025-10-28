using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour
{
    public bool isEating = false;
    private bool isAlreadyEating = false;
    private bool seedsLeft = true;
    public GameObject assignatedSeed;
    private SeedPit seedPitClass;
    void Start()
    {
        seedPitClass = assignatedSeed.GetComponent<SeedPit>();
    }

    public void CaughtBird() // Function called when the bird is removed by the player
    {
        isEating = false;
        isAlreadyEating = false;
        StartCoroutine(SmoothMoveToPosition(new Vector3(gameObject.transform.position.x, assignatedSeed.transform.position.y + 30, gameObject.transform.position.z))); // Move smoothly to a new position
        //StartCoroutine(ReturningBird()); // validate if functional
        Debug.Log("Bird is caught!");
        // Add animation fly away
    }
    public void StartEatingCycle() // Function called when the bird is triggered/activated
    {
        if (seedsLeft == true)
        {
            StartCoroutine(ReturningBird());
        }
    }
    private IEnumerator EatingBird() // Bird eating seed pit
    {
        if (isAlreadyEating == true)
        {
            yield break;
        }
        while (isEating == true && seedsLeft == true)
        {
            seedPitClass.BirdDanger();
            isAlreadyEating = true;
            // start eating animation
            yield return new WaitForSeconds(2f);
            if (seedsLeft == true && isEating == true)
            {
                seedPitClass.seedPitLoseHp(1);
            }
            else if (seedsLeft == false)
            {
                SeedsAreGone();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator ReturningBird() // Bird goes back to the seed pit
    {
        if (seedsLeft == true)
        {
            yield return new WaitForSeconds(5);
            DescendingBird();
            yield return new WaitForSeconds(1);
            StartCoroutine(EatingBird()); //add loop here
        }
    }
    private void DescendingBird()
    {
        Debug.Log("Bird has descended");
        StartCoroutine(SmoothMoveToPosition(new Vector3(gameObject.transform.position.x, assignatedSeed.transform.position.y - 0.54f, gameObject.transform.position.z)));
        isEating = true;
        // animation fly in
    }

    public void SeedsAreGone()
    {
        seedsLeft = false;
        CaughtBird();
        // call animation flying away
    }

    private IEnumerator SmoothMoveToPosition(Vector3 targetPosition) // temp 
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = transform.position;

        // Move the bird smoothly over time to the target position
        while (timeElapsed < 1f)  // 1 second for the full movement, adjust as needed
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, timeElapsed);  // Smoothly interpolate the position
            timeElapsed += Time.deltaTime * 5;  // Adjust speed here (larger = faster)
            yield return null;
        }

        transform.position = targetPosition;  // Ensure the bird is exactly at the target position
    }

}
