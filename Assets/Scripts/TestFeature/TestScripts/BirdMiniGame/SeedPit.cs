using TMPro;
using UnityEngine;
using System.Collections;

public class SeedPit : MonoBehaviour
{
    private int seedPitHp = 10;
    public TextMeshProUGUI hpPitUi;
    [SerializeField] GameObject[] birdArray; // temp test

    [SerializeField] BirdManager _birdManagerScript;
    [SerializeField] float takeDamageRedDuration = 1;
    private int birdIndex = 0;
    private void Start() // Temp
    {
        UpdatePitHp(); // temp
        GetComponent<MeshRenderer>().materials[0].color = Color.green;

        //birdArray.GetComponent<BirdBehavior>().StartEatingCycle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var bird in birdArray)
            {
                bird.GetComponent<BirdBehavior>().CaughtBird(); 
            }
        }
        GetComponent<MeshRenderer>().materials[0].color = Color.green;
    }
    public void seedPitLoseHp(int hpLost)
    {
        seedPitHp -= hpLost;
       // Debug.Log("Badabim");
        UpdatePitHp();
        StartCoroutine(TakeDamageAnimation());

        if (seedPitHp <= 0)
        {
            //birdArray.GetComponent<BirdBehavior>().SeedsAreGone(); // call other birds eventually
            SeedsAreGone();
        }
        
        
    }

    private IEnumerator TakeDamageAnimation()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.red;
        yield return new WaitForSeconds(takeDamageRedDuration);
        GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
        yield return null;

    }
    public void BirdDanger()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
        

    }


    private void UpdatePitHp()
    {
        hpPitUi.text = seedPitHp.ToString();
    }

    public void CallNewBird()
    {
        int nextBirdIndex = 0;

        while (birdIndex + nextBirdIndex < birdArray.Length)
        {
            var bird = birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>();

            if (bird.isEating == false) 
            {
                bird.StartEatingCycle(); 
                break; 
            }

            nextBirdIndex++; 
        }

        //int nextBirdIndex = 0;
        //if (birdIndex <= 4) // Pits have a limit of 5 birds
        //{
        //    if (birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>().isEating == false) // Activates bird
        //    {
        //        birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>().StartEatingCycle(); // Activates bird            
        //    }
        //    else if (birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>().isEating == true)
        //    {
        //        nextBirdIndex += 1;
        //        if (birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>().isEating == false) // Activates bird
        //        {
        //            birdArray[birdIndex + nextBirdIndex].GetComponent<BirdBehavior>().StartEatingCycle(); // Activates bird            
        //        }
        //    }
        //    //birdIndex += 1; // Next bird will be summoned
        //}

        //else
        //{
        //    _birdManagerScript.UnavailaiblePit(this.gameObject); // removes the seed pit from the list of available pits.
        //}

    }

    private void SeedsAreGone()
    {
        foreach (var bird in birdArray)
        {
            bird.GetComponent<BirdBehavior>().SeedsAreGone(); // call other birds eventually
        }
    }
}
