using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Gamestate gamestate;
    public static Gamestate Gamestate
    {
        get { return gamestate; }
        set { previousGamestate = gamestate; gamestate = value; }
    }
    public static Gamestate previousGamestate;
    public static GameManager instance;

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

