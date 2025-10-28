using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;  



public class DragDropMiniGame : MonoBehaviour
{
    [Header("Mini-Game Parameters")]
    [SerializeField] int goodSeedsOfEachType; // Per basket
    [SerializeField] int badSeedsAmount;
    [SerializeField] int allowedPlayerMistakes = 5;
    private int currentPlayerMistakes;


    [Header("Seeds related informations")]
    public List<Seed> totalSeedList = new List<Seed>();
    [SerializeField] Seed currentSeed; // Currently selected seed


    //[SerializeField] GameObject iconDisplay;
    [Header("Current Selected seed images")]
    public Image iconDisplayImage;

    [SerializeField] Sprite bananaImage;
    [SerializeField] Sprite watermelonImage;
    [SerializeField] Sprite pumpkinImage;
    [SerializeField] Sprite sunflowerImage;
    [SerializeField] Sprite noDisplayImage;
    [SerializeField] Sprite[] badSeedImages;

    [Header("Cursor icon images")]

    [SerializeField] Texture2D bananaCursorImage;
    [SerializeField] Texture2D waterMelonCursorImage;
    [SerializeField] Texture2D sunflowerCursorImage;
    [SerializeField] Texture2D pumpkinCursorImage;
    [SerializeField] Texture2D[] badSeedCursorImages; 


    [Header("Basket GameObjects icon images")]
    [SerializeField] GameObject pumpkinBasket;
    [SerializeField] GameObject bananaBasket;
    [SerializeField] GameObject sunflowerBasket;
    [SerializeField] GameObject watermelonBasket;
    [SerializeField] GameObject trashBasket;
    [SerializeField] GameObject voidBasket;

    [Header("Mini-game UI elements")]
    [SerializeField] TextMeshProUGUI seedCountText;
    [SerializeField] TextMeshProUGUI mistakesLeftText;


    [Header("End Screens")]
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;



    public enum Seed
    {
        waterMelon,
        banana,
        pumpkin,
        sunflower,
        badSeed,
        none
    }

    private void Start() // Add the same amount of seed for each basket (if that's what we want) - LCC
    {
        initializeSeeds();

        if (seedCountText != null)
        {
        UpdateUI();

        }    

    }
    private void OnMouseDown()
    {
        int currentCount = totalSeedList.Count;
        Debug.Log(currentCount);
        if (currentCount > 0) // Validation so player doesn't draw from empty basket
        {
            int drawnSeed = UnityEngine.Random.Range(0, totalSeedList.Count); // Picks a seed in the basket

            currentSeed = totalSeedList[drawnSeed];
            UpdateCurrentSeed(currentSeed); // Updates user feedback
            Debug.Log("current seed is : " + currentSeed);
        }
    }

    private void OnMouseUp()
    {

        if (IsSeedOverRightBasket())
        {
           // Debug.Log("You did it!");
            UpdateSeedList(currentSeed);
        }

        else if (IsSeedOverWrongBasket())
        {
            Debug.Log("The " + currentSeed +"was placed in the wrong basket");
            currentPlayerMistakes += 1;
            if (allowedPlayerMistakes - currentPlayerMistakes <= 0)
            {
                loseScreen.SetActive(true);
                Debug.Log("Player has lost");
            }
            UpdateSeedList(currentSeed);
        }
        UpdateUI();

        currentSeed = Seed.none;
        UpdateCurrentSeed(currentSeed);

        if (totalSeedList.Count == 0) // victory condition
        {
            winScreen.SetActive(true);
        }
    }

    private void UpdateCurrentSeed(Seed currentSeed)
    {   
        UpdateSeedDisplay(currentSeed);
    }

    private void UpdateUI()
    {
        int currentAmountOfSeedsLeft = totalSeedList.Count;
        seedCountText.text = currentAmountOfSeedsLeft.ToString();
        mistakesLeftText.text = (allowedPlayerMistakes - currentPlayerMistakes).ToString();
    }    
    private void UpdateSeedList(Seed currentSeed)
    {
        totalSeedList.Remove(currentSeed);
    }

    private bool IsSeedOverRightBasket()
    {
       
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            //Debug.Log("Touched something" + result.gameObject.name);
            if (result.gameObject == TargetZone())
            {
                return true; // Cursor is over the correct target object
            }

            //else if (currentSeed == Seed.badSeed) // Do we punish player for putting the wrong seed in the wrong basket?
            else // currently, placing a seed NOT over right basket will lead to loss (is that what we want?)
            {
                return false;
            }
        }
        return false;
    }

    private bool IsSeedOverWrongBasket()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            // Check if the cursor is over a wrong basket
            if ((result.gameObject == watermelonBasket && currentSeed != Seed.waterMelon) ||
                (result.gameObject == bananaBasket && currentSeed != Seed.banana) ||
                (result.gameObject == pumpkinBasket && currentSeed != Seed.pumpkin) ||
                (result.gameObject == sunflowerBasket && currentSeed != Seed.sunflower) || result.gameObject == trashBasket && currentSeed != Seed.badSeed)
            {
                return true; // Seed is placed in the wrong basket
            }
        }

        return false; // Not in a wrong basket
    }
    private GameObject TargetZone()
    {
        if (currentSeed == Seed.waterMelon)
        {
            return watermelonBasket;
        }
        if (currentSeed == Seed.sunflower)
        {
            return sunflowerBasket;
        }
        if (currentSeed == Seed.pumpkin)
        {
            return pumpkinBasket;
        }
        if (currentSeed == Seed.banana)
        {
            return bananaBasket;
        }
        if (currentSeed == Seed.badSeed)
        {
            return trashBasket;
        }
        else
        {
            return voidBasket;
        }

    }


  

    private void UpdateSeedDisplay(Seed currentSeed) // Changes the display image depending on current selected seed
    {

        if (currentSeed == Seed.banana)
        {
            iconDisplayImage.sprite = bananaImage;
            Cursor.SetCursor(bananaCursorImage, Vector2.zero, CursorMode.Auto);
        }
        if (currentSeed == Seed.waterMelon)
        {
            iconDisplayImage.sprite = watermelonImage;
            Cursor.SetCursor(waterMelonCursorImage, Vector2.zero, CursorMode.Auto);

        }
        if (currentSeed == Seed.pumpkin)
        {
            iconDisplayImage.sprite = pumpkinImage;
            Cursor.SetCursor(pumpkinCursorImage, Vector2.zero, CursorMode.Auto);

        }
        if (currentSeed == Seed.sunflower)
        {
            iconDisplayImage.sprite = sunflowerImage;
            Cursor.SetCursor(sunflowerCursorImage, Vector2.zero, CursorMode.Auto);

        }
        if (currentSeed == Seed.none)
        {
            iconDisplayImage.sprite = noDisplayImage;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }

        if (currentSeed == Seed.badSeed)
        {
            int randomImage = Random.Range(0, badSeedCursorImages.Length);
            iconDisplayImage.sprite = badSeedImages[randomImage];
            Cursor.SetCursor(badSeedCursorImages[randomImage], Vector2.zero, CursorMode.Auto);

        }

    }

    private void initializeSeeds()
    {
        for (int i = 0; i < goodSeedsOfEachType; i++)
        {
            totalSeedList.Add(Seed.waterMelon);
        }
        for (int i = 0; i < goodSeedsOfEachType; i++)
        {
            totalSeedList.Add(Seed.banana);
        }
        for (int i = 0; i < goodSeedsOfEachType; i++)
        {
            totalSeedList.Add(Seed.pumpkin);
        }
        for (int i = 0; i < goodSeedsOfEachType; i++)
        {
            totalSeedList.Add(Seed.sunflower);
        }
        for (int i = 0; i < badSeedsAmount; i++) // adding more bad seeds
        {
            totalSeedList.Add(Seed.badSeed);
        }
    }    
}
