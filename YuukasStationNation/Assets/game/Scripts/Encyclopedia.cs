using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia instance;
    [Header("pages")]
    [SerializeField] private int pageNumber = 0;
    [SerializeField] private EncyclopediaGameobject leftPage;
    [SerializeField] private EncyclopediaGameobject rightPage;
    [SerializeField] private List<EncyclopediaPage> currentPages;

    [Header("Buttons")]
    [SerializeField] private Color buttonEnabledColour;
    [SerializeField] private Vector2 buttonPosOffset;
    [SerializeField] private List<Button> buttonBiomesList;
    [SerializeField] private List<Vector3> buttonBasePosList;
    [SerializeField] private EncyclopediaButton pressingButton;
    [SerializeField] private EncyclopediaButton depressingButton;

    [Header("Timer")]
    [SerializeField] private float duration;
    [SerializeField] private GlobalTimer buttonMovementTimer = null;
    bool hasStarted;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        GameManager.instance.CreateFishPages();

        buttonBasePosList = new Vector3[buttonBiomesList.Count].ToList();
        for (int i = 0; i < buttonBiomesList.Count; i++)
        {
            buttonBasePosList[i] = buttonBiomesList[i].transform.position;
        }
        for (int i = 0; i < Enum.GetNames(typeof(Biome)).Length; i++)
        {
            // this variable is essential to make the thing not explode. I dont fully know why but... it just works
            int count = i;
            Debug.Log($"interpreting {buttonBiomesList[count].name} as {(Biome)count} with i {count}, ");

            buttonBiomesList[i].onClick.AddListener(() => OnButtonPress(buttonBiomesList[count], (Biome)count));
        }
        buttonMovementTimer = null;
        pressingButton = null;
        depressingButton = null;

        ChangePageMode();
        RefreshPages();
        if (buttonBiomesList.Count != Enum.GetNames(typeof(Biome)).Length)
        {
            Debug.LogError("number of buttons dont match number of biomes for encyclopedia.cs");
        }
        hasStarted = true;
    }


    private void OnEnable()
    {
        if (hasStarted)
        {
            RefreshPages();
            if (buttonBiomesList.Count != Enum.GetNames(typeof(Biome)).Length)
            {
                Debug.LogError("number of buttons dont match number of biomes for encyclopedia.cs");
            }
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

        UpdatePages(pageNumber);
    }


    private void UpdatePages(int pageNumberOfLeft)
    {
        // Check if the second page should be disabled
        Debug.Log($"page count:{currentPages.Count}");
        leftPage.UpdateWithNewPage(currentPages[pageNumberOfLeft]);

        if (pageNumberOfLeft + 1 >= currentPages.Count)
        {
            rightPage.everythingInfo.SetActive(false);
        }
        else
        {
            rightPage.everythingInfo.SetActive(true);
            rightPage.UpdateWithNewPage(currentPages[pageNumberOfLeft + 1]);
        }
    }


    /// <summary>
    /// resets the page sorting mode to include all biomes
    /// </summary>
    public void ChangePageMode()
    {
        currentPages.Clear();
        foreach (EncyclopediaPage page in GameManager.instance.fishEncyclopediaList)
        {
            currentPages.Add((EncyclopediaPage)page.Clone());
        }
        pageNumber = 0;
        UpdatePages(pageNumber);
    }


    /// <summary>
    /// resets the page sorting mode to include only specific biome
    /// </summary>
    /// <param name="biome"></param>
    public void ChangePageMode(Biome biome)
    {
        currentPages.Clear();
        foreach (EncyclopediaPage page in GameManager.instance.fishEncyclopediaList)
        {
            if (page.Fish.biome == biome)
                currentPages.Add((EncyclopediaPage)page.Clone());
        }
        pageNumber = 0;
        UpdatePages(pageNumber);
    }


    public void RefreshPages()
    {
        TurnPage(0);
    }


    public void OnButtonPress(Button button, Biome sortMode)
    {
        Debug.Log($"buttonPressed by {button.gameObject.name} with sorting mode {sortMode}", button.gameObject);
        //only activate when timer doesnt exist
        if (buttonMovementTimer != null && buttonMovementTimer.PercentFinished > 0) return;

        buttonMovementTimer = TimerConstructor();
        if (pressingButton != null && pressingButton.button != null)
        {
            pressingButton.ReverseInfo();
            depressingButton = pressingButton;
            depressingButton.AttachNewTimer(buttonMovementTimer);
            pressingButton = null;
        }


        //If its an already pressed button, then unpress it.
        if (button == depressingButton?.button)
        {
            ChangePageMode();
        }
        //if its a new button then press it down and depress the previous one
        else if (button != depressingButton?.button || depressingButton == null)
        {
            pressingButton = new EncyclopediaButton
            (
                button,
                button.transform.position + (Vector3)buttonPosOffset,
                buttonEnabledColour,
                buttonMovementTimer
            );

            ChangePageMode(sortMode);
        }
        buttonMovementTimer.Restart();

    }


    public GlobalTimer TimerConstructor()
    {
        buttonMovementTimer = new GlobalTimer(duration);
        buttonMovementTimer.callOnFinish += TimerDestructor;
        return buttonMovementTimer;
    }


    public void TimerDestructor()
    {
        buttonMovementTimer?.Dispose();
        buttonMovementTimer = null;
    }

}


[Serializable]
public class EncyclopediaButton : IDisposable
{
    public Button button;
    public Vector3 startVector;
    public Vector3 endVector;
    public List<Color> startColours = new();
    public List<Color> pressedColours = new();
    public GlobalTimer timer;
    private RectTransform rectTransform;

    public List<Image> graphicsOfButton = new List<Image>();


    public EncyclopediaButton(Button newButton, Vector3 newEndVector, Color newAdditiveColour, GlobalTimer newTimer)
    {
        button = newButton;
        rectTransform = button.GetComponent<RectTransform>();
        startVector = rectTransform.anchoredPosition;
        endVector = newEndVector;

        timer = newTimer;

        graphicsOfButton = button.gameObject.transform.GetComponentsInChildren<Image>().ToList<Image>();
        Debug.Log(graphicsOfButton.Count);
        for (int i = 0; i < graphicsOfButton.Count; i++)
        {
            startColours.Add(graphicsOfButton[i].color);
            pressedColours.Add(graphicsOfButton[i].color += newAdditiveColour);
        }
        Debug.Log($"pre pre attaching timers {graphicsOfButton.Count}");
        AttachNewTimer(timer);
        Debug.Log($"post post attaching timers {graphicsOfButton.Count}");
    }

    public void AttachNewTimer(GlobalTimer newTimer)
    {
        Debug.Log($"pre clearing timer {graphicsOfButton.Count}");
        clearThings();
        timer = newTimer;
        timer.callOnStart += () => OnUpdate(0, this);
        timer.callOnUpdate += () => OnUpdate(timer.PercentFinished, this);
        timer.callOnFinish += () => OnUpdate(1, this);
        Debug.Log($"post attaching timers {graphicsOfButton.Count}");
    }

    public void ChangeVectors(Vector3 newStartVector, Vector3 newEndVector)
    {
        startVector = newStartVector;
        endVector = newEndVector;
    }

    public void ReverseInfo()
    {

        List<Color> temp;
        temp = startColours;
        startColours = pressedColours;
        pressedColours = temp;

        Vector3 coolerTemp;
        coolerTemp = startVector;
        startVector = endVector;
        endVector = coolerTemp;
    }

    public void OnUpdate(float percentFinished, EncyclopediaButton instance)
    {

        Vector3 vector = Vector3.Lerp(startVector, endVector, percentFinished);

        Debug.Log($"percent finished: {percentFinished}");

        EditorApplication.isPaused = true;

        Debug.Log(instance.graphicsOfButton.Count);
        for (int i = 0; i < instance.graphicsOfButton.Count; i++)
        {
            Color colour = Color.Lerp(startColours[i], pressedColours[i], percentFinished);
            graphicsOfButton[i].color = colour;
        }
        Debug.LogWarning($"{button.name}");
        button.transform.position = vector;

    }

    ~EncyclopediaButton()
    {
        Dispose();
    }
    public void Dispose()
    {
        if (timer != null)
        {
            timer.callOnStart -= () => OnUpdate(0, this);
            timer.callOnUpdate -= () => OnUpdate(timer.PercentFinished, this);
            timer.callOnFinish -= () => OnUpdate(1, this);
        }
    }
    public void clearThings()
    {
        if (timer != null)
        {
            timer.callOnStart -= () => OnUpdate(0, this);
            timer.callOnUpdate -= () => OnUpdate(timer.PercentFinished, this);
            timer.callOnFinish -= () => OnUpdate(1, this);
        }
    }
}
