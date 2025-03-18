using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia instance;
    [SerializeField] private int pageNumber = 0;
    [SerializeField] private EncyclopediaGameobject leftPage;
    [SerializeField] private EncyclopediaGameobject rightPage;
    [SerializeField] private List<EncyclopediaPage> currentPages;
    [SerializeField] private Color buttonEnabledColour;
    [SerializeField] private Vector2 buttonPosOffset;
    [SerializeField] private List<Button> buttonBiomesList;
    [SerializeField] private List<Vector3> buttonBasePosList;
    [SerializeField] private Button pressedButton;
    // Start is called before the first frame update

    [Header("Timer")]

    [SerializeField] private float duration;
    [SerializeField] private GlobalTimer buttonMovementTimer = new GlobalTimer();
    private GlobalTimer ButtonMovementTimer
    {
        get;
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < buttonBiomesList.Count; i++)
        {
            buttonBasePosList[i] = buttonBiomesList[i].transform.position;
        }
    }

    private void OnEnable()
    {
        RefreshPages();
        if (buttonBiomesList.Count != Enum.GetNames(typeof(Biome)).Length)
        {
            Debug.LogError("number of buttons dont match number of biomes for encyclopedia.cs");
        }
    }
    public void TurnPage(int offset)
    {
        // Calculate the new page number
        int newPageNumber = pageNumber + offset;

        // Ensure the new page number is within valid bounds
        if (newPageNumber < 0)
        {
            newPageNumber = 0; // Clamp to the first page
        }
        else if (newPageNumber >= currentPages.Count)
        {
            // Clamp to the last even page number
            newPageNumber = currentPages.Count - 1;
            if (newPageNumber % 2 != 0)
            {
                newPageNumber--; // Ensure it's even
            }
        }

        // Ensure the page number is always even
        if (newPageNumber % 2 != 0)
        {
            newPageNumber--; // Adjust to the previous even number
        }

        // Update the page number
        pageNumber = newPageNumber;

        // Check if the second page should be disabled
        leftPage.UpdateWithNewPage(currentPages[pageNumber]);

        if (pageNumber + 1 >= currentPages.Count)
        {
            rightPage.everythingInfo.SetActive(false);
        }
        else
        {
            rightPage.UpdateWithNewPage(currentPages[pageNumber+1]);
            rightPage.everythingInfo.SetActive(true);
        }
    }

    public void ChangePageMode()
    {
        currentPages.Clear();
        currentPages = GameManager.instance.fishEncyclopediaList;
        pageNumber = 0;
    }
    public void ChangePageMode(Biome biome)
    {
        currentPages.Clear();
        foreach (EncyclopediaPage page in GameManager.instance.fishEncyclopediaList)
        {
            if (page.Fish.biome == biome)
                currentPages.Add(page);
        }
        pageNumber = 0;
    }

    public void RefreshPages()
    {
        TurnPage(0);
    }

    public void OnButtonPress(Button button, Biome sortMode)
    {
        //just deactivate the button and reset to all sort mode
        if (button == pressedButton)
        {
            pressedButton = null;
        }
        else
        {

        }

        //deactive pressedbutton and then switchitout for the new button that was pressed
    }

    public void TimerSelfDestructor()
    {
        
    }

}
