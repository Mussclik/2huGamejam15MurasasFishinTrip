using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FishRecordKeeper : MonoBehaviour
{
    public static FishRecordKeeper instance;


    [SerializeField] public List<FishObject> allPossibleFish;
    [SerializeField] public List<FishEntry> allCaughtFish;

    private void Awake()
    {
        instance = this;
    }

    public bool IsCaughtFishSignificant(FishObject newFish, out FishSignifigance fishSignifigance)
    {
        bool preExistent = false;
        fishSignifigance = FishSignifigance.None;
        for (int i = 0; i < allCaughtFish.Count;)
        {
            if (allCaughtFish[i].fish.fishID == newFish.fishID)
            {
                preExistent = true;
                if (IsNewFishHeavier(newFish, allCaughtFish[i].fish))
                {
                    fishSignifigance = FishSignifigance.IsRecordHeavyFish;
                    return true;
                }
                else if (IsNewFishLarger(newFish, allCaughtFish[i].fish))
                {
                    fishSignifigance = FishSignifigance.IsRecordLargeFish;
                    return true;
                }

            }
        }
        if (!preExistent)
        {
            fishSignifigance = FishSignifigance.IsNewFish;
            return true;
        }
        else
        {

            return false;
        }

    }

    private bool IsNewFishLarger(FishObject newFish, FishObject oldFish)
    {
        if (newFish.Size > oldFish.Size) return true;
        else return false;
    }
    private bool IsNewFishHeavier(FishObject newFish, FishObject oldFish)
    {
        if (newFish.Weight > oldFish.Weight) return true;
        else return false;
    }
}


[Serializable]
public class FishEntry
{
    public FishObject fish;
    public float biggestSize = 0;
    public float heaviestWeight = 0;
    public int amountCaught = 0;
}
public enum FishSignifigance
{
    None,
    IsNewFish,
    IsRecordHeavyFish,
    IsRecordLargeFish,

}