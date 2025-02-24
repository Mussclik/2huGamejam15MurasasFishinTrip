using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Fishing/Upgrade Object")]
public class UpgradeObject : ScriptableObject
{
    public int difficultyChange = 1;
    public float fishingSpeedModifier = 1;
    public bool CanFishInLava;
    public float price;
    public string upgradeName;
    public string upgradeDescription;
}
