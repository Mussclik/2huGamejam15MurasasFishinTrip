using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaPage : MonoBehaviour
{
    public int amountCaught = 0;
    public Biome biome;
    public int idOfFish;
    public FishObject fish;

    public float largestSize = 0f;
    public float largestWeight = 0f;

    public GameObject informationDisplayObject;

    private void OnEnable()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        //if (idOfFishhcsd == -1) return;

    }
    
    public bool RegisterFishCaught()
    {
        throw new System.Exception("Not Implemented");
    }
}

public enum Biome
{
    Normal,
    BloodWhells,
    Strange,
}