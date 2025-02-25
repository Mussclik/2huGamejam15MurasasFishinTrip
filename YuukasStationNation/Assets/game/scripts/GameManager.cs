using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Gamestate gamestate;
    [SerializeField] private bool pauseMenuIsOpen;
    public static Gamestate Gamestate
    {
        get { return gamestate; }
        set { previousGamestate = gamestate; gamestate = value; }
    }
    public static Gamestate previousGamestate;
    public static GameManager instance;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayMusic(1, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            MenuCheck();
        }
    }
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

