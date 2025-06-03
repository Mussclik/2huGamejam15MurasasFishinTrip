using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour, ITimerStaticAttachable
{
    private static Gamestate gamestate;
    public event System.Action updateTimer = () => { };
    public System.Action UpdateTimer
    {
        get { return updateTimer; }
        set { updateTimer = value; }
    }


    private void Awake()
    {
        instance = this;
    }
    
    
    void Start()
    {
        SoundManager.instance.PlayMusic(1, true);
        encyclopediaButton.onClick.AddListener(ToggleEncyclopediaState);

        ITimerStaticAttachable.AttachUpdatingScript(this);

        ToggleEncyclopediaState(false);
        ToggleMenuState(true);
    }


    public void DeclareAction()
    {

    }


    void Update()
    {
        if (updateTimer != null) updateTimer();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            MenuCheck();
        }
    }


    #region PlayerStatesFunctions
    [Header("PlayerStates")]
    [SerializeField] private bool pauseMenuIsOpen;
    [SerializeField] private GameObject pauseMenu;


    public static Gamestate Gamestate
    {
        get { return gamestate; }
        set { previousGamestate = gamestate; gamestate = value; }
    }
    public static Gamestate previousGamestate;
    public static GameManager instance;
    public static bool isAnimationsDisabled;



    // Start is called before the first frame update

    public void MenuCheck()
    {
        if (pauseMenuIsOpen)
        {
            pauseMenu.SetActive(false);
            pauseMenuIsOpen = false;
            TimerCheck();
            encyclopediaButton.gameObject.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(true);
            pauseMenuIsOpen = true;
            TimerCheck();

            if (encyclopediaMenuIsOpen)
            {
                ToggleEncyclopediaState();
            }
            encyclopediaButton.gameObject.SetActive(false);
        }
    }
    public void ToggleMenuState(bool newState)
    {
        if (!newState)
        {
            pauseMenu.SetActive(false);
            pauseMenuIsOpen = false;
            encyclopediaButton.gameObject.SetActive(true);
            TimerCheck();
        }
        else
        {
            pauseMenu.SetActive(true);
            pauseMenuIsOpen = true;
            if (encyclopediaMenuIsOpen)
            {
                ToggleEncyclopediaState();
            }
            encyclopediaButton.gameObject.SetActive(false);
            TimerCheck();
        }
    }

    public void TimerCheck()
    {
        if (pauseMenuIsOpen || encyclopediaMenuIsOpen)
        {
            Time.timeScale = 0.000001f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    #endregion

    #region EncyclopediaFunctions

    [Header("Encyclopedia ")]
    [SerializeField] private bool encyclopediaMenuIsOpen;
    [SerializeField] private GameObject encyclopediaMenu;
    [SerializeField] private Button encyclopediaButton;

    [Header("FishList")]
    [SerializeField] public List<FishObject> fishList;
    [SerializeField] public List<EncyclopediaPage> fishEncyclopediaList;


    public void ToggleEncyclopediaState()
    {
        if (!encyclopediaMenuIsOpen)
        {
            encyclopediaButton.gameObject.SetActive(false);
            encyclopediaMenu.SetActive(true);
            encyclopediaMenuIsOpen = true;
            TimerCheck();
        }
        else
        {
            encyclopediaButton.gameObject.SetActive(true);
            encyclopediaMenu.SetActive(false);
            encyclopediaMenuIsOpen = false;
            TimerCheck();
        }
    }
    public void ToggleEncyclopediaState(bool newValue)
    {
        if (newValue)
        {
            encyclopediaButton.gameObject.SetActive(false);
            encyclopediaMenu.SetActive(true);
            encyclopediaMenuIsOpen = true;
            TimerCheck();
        }
        else
        {
            encyclopediaButton.gameObject.SetActive(true);
            encyclopediaMenu.SetActive(false);
            encyclopediaMenuIsOpen = false;
            TimerCheck();
        }
    }


    public int SortFishByBiome(FishObject firstFish, FishObject secondFish)
    {
        return ((int)firstFish.biome).CompareTo((int)secondFish.biome);
    }
    
    
    public void CreateFishPages()
    {
        fishList.Sort(SortFishByBiome);

        for (int i = 0; i < fishList.Count; i++)
        {
            fishList[i].fishID = i;
            EncyclopediaPage newPage = new EncyclopediaPage(i);

            fishEncyclopediaList.Add(newPage);
        }
        Debug.Log("häng miug");
    }


    public FishObject GetFish(int id)
    {
        if (id < 0)
        {
            FishObject newFish = new FishObject();
            newFish.fishName = "man, someone is broken :(";
            newFish.description = "man, someone like very broken :(";
            newFish.refrence = "my bad skills";
            newFish.fishID = id;
            return newFish;
        }
        return fishList[id];
    }


    public FishObject GetFishByBiome(Biome biome, int fishingStrength = 0, bool isEffectedByStrength = false)
    {
        List<FishObject> potentialFish = new List<FishObject>();
        foreach (FishObject fish in fishList)
        {
            bool validFish = true;
            if (fish.biome != biome)
            {
                validFish = false;
            }
            if (isEffectedByStrength && fishingStrength != fish.difficulty)
            {
                validFish = false;
            }

            if (validFish)
            {
                potentialFish.Add(fish);
            }
        }

        if (potentialFish.Count >= 1)
        {
            return potentialFish[UnityEngine.Random.Range(0, potentialFish.Count)];
        }
        else
        {
            return null;
        }
    }
    
    #endregion

}


public enum Gamestate
{
    MovingOnMap,
    Fishing,
    ActivelyCatchingFish,
    LookingAtFishCaught,
    InAnimation,
    InIngameMenu,
    InPauseMenu,
    InTransition,
}


public enum Biome
{
    Normal = 0,
    BloodWhells = 1,
    Strange = 2,
}

