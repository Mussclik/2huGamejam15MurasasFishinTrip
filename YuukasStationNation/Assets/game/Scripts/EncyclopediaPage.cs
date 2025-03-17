using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class EncyclopediaPage
{
    public int amountCaught = 0;
    public int fishID = -99;


    public float largestSize = 0f;
    public float largestWeight = 0f;

    public EncyclopediaPage(int givenID)
    {
        fishID = givenID;
    }

    public FishObject Fish
    {
        get
        {
            return GameManager.instance.GetFish(fishID);
        }
    }


    /// <summary>
    /// Returns (isNewLargestFish, isNewHeaviestFish) and registers the fish into the index.
    /// </summary>
    /// <param name="newFish"></param>
    /// <returns></returns>
    public (bool, bool) RegisterFishCaught(FishObject newFish)
    {
        bool newlargestSize = false;
        bool newlargestWeight = false;
        
        amountCaught++;
        if (largestSize < newFish.Size)
        {
            largestSize = newFish.Size;
            newlargestSize = true;
        }
        if (largestWeight < newFish.Weight)
        {
            largestWeight = newFish.Weight;
            newlargestWeight = true;
        }
        return (newlargestSize, newlargestWeight);
    }
}

