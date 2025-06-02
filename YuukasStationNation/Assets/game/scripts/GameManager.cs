using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITimerStaticAttachable
{
    private static Gamestate gamestate;
    public static Action onUpdateStatic;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SoundManager.instance.PlayMusic(1, true);
        Time.timeScale = 0.000001f;
    }

    public void DeclareAction()
    {

    }

    void Update()
    {
        if (ITimerStaticAttachable.onUpdateStatic != null) ITimerStaticAttachable.onUpdateStatic();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            MenuCheck();
        }
    }


    #region PlayerStatesFunctions
    [SerializeField] private bool pauseMenuIsOpen;
    public static Gamestate Gamestate
    {
        get { return gamestate; }
        set { previousGamestate = gamestate; gamestate = value; }
    }
    public static Gamestate previousGamestate;
    public static GameManager instance;
    public static bool isAnimationsDisabled;
    [SerializeField] private GameObject pauseMenu;



    // Start is called before the first frame update

    public void MenuCheck()
    {
        if (pauseMenuIsOpen)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            pauseMenuIsOpen = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0.000001f;
            pauseMenuIsOpen = true;
        }
    }
    #endregion

    #region EncyclopediaFunctions
    [SerializeField] public List<FishObject> fishList;
    [SerializeField] public List<EncyclopediaPage> fishEncyclopediaList;


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
        List<FishObject> potencialFish = new List<FishObject>();
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
                potencialFish.Add(fish);
            }
        }

        if (potencialFish.Count >= 1)
        {
            return potencialFish[UnityEngine.Random.Range(0, potencialFish.Count)];
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

