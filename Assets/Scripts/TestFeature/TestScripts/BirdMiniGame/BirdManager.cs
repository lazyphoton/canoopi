using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public List<GameObject> SeedPitList = new List<GameObject>();
    [SerializeField] int numberOfPitsToSave = 6;
    [SerializeField] float timeBetweenBirds = 0.8f;
    [SerializeField] int maxBirdSummoned = 3; // RandomRange exclusif, donc 3 = 2 bird max
    private int amountsOfPitBeforeLosing;
    private int maximumAmountOfBirds;
    private int currentAmountOfBirds = 0;
    private int currentWave = 0;
    private bool miniGameStarted = false;
    private int birdsToCall = 3;


    [Header("Wave Management")]
    [SerializeField] int[] waveBirdAmountIncrease;
    [SerializeField] int waveMax;

    private void Start()
    {
        int amountOfSeedPits = SeedPitList.Count;
        amountsOfPitBeforeLosing = amountOfSeedPits - numberOfPitsToSave;
        maximumAmountOfBirds = amountOfSeedPits * 5; // 5 birds per pod for now)
    }
    private void AwakenBird()
    {
        if (currentAmountOfBirds < maximumAmountOfBirds)
        {
            int randomPodIndex = Random.Range(0, SeedPitList.Count);
            SeedPitList[randomPodIndex].GetComponent<SeedPit>().CallNewBird();

        }
    }
    private void SummonMoreBirds()
    {
        if (currentWave == waveBirdAmountIncrease[0] || currentWave == waveBirdAmountIncrease[1] || currentWave == waveBirdAmountIncrease[2] ||  currentWave == waveBirdAmountIncrease[3] || currentWave == waveBirdAmountIncrease[4])
        {
            birdsToCall += 3;
            timeBetweenBirds -= 0.1f;

            if (currentWave == waveBirdAmountIncrease[2])
            {
                maxBirdSummoned = maxBirdSummoned + 1;
            }
            if (currentWave == waveBirdAmountIncrease[4])
            {
                maxBirdSummoned = maxBirdSummoned + 1;
            }
        }
    }

    public void UnavailaiblePit(GameObject unavailablePit) // Pits become unavailable if they have no more seeds or if they have enough 
    {
        if (SeedPitList.Contains(unavailablePit)) // Check current list of seedPits for seedPit presence, then remove them
        {
            SeedPitList.Remove(unavailablePit);
        }
        if (SeedPitList.Count <= amountsOfPitBeforeLosing)
        {
            MiniGameLost();
        }
    }
    public void MiniGameStarts()
    {
        if (miniGameStarted == false)
        {

            Debug.Log("Starting whack a mole bird minigame");
            miniGameStarted = true;
            StartCoroutine(MiniGameBehavior());
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) // Temp
        //{
        //    MiniGameStarts();
        //}
    }

    private void MiniGameLost()
    {
        Debug.Log("Player has lost");
        miniGameStarted = false;
        // Add losing effect here
    }


    private IEnumerator MiniGameBehavior() // Bird eating seed pit
    {
        while (miniGameStarted == true)
        {
            // start eating animation
            yield return new WaitForSeconds(3);

            currentWave += 1;



            SummonMoreBirds();

            int birdsSummoned = 0;


            if (miniGameStarted == true && currentWave <= waveMax)
            {
                Debug.Log("New bird wave dropping, current wave is " + currentWave);
                while (birdsSummoned < birdsToCall)
                {
                    int birdsSummonedThisTime = Random.Range(1, maxBirdSummoned);
                    AwakenBird();              
                    birdsSummoned += birdsSummonedThisTime;           
                    yield return new WaitForSeconds(timeBetweenBirds);
                }


            }

            yield return null;
        }
    }

}

