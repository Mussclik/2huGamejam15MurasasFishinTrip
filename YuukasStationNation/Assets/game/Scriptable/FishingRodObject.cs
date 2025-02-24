using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FishingRod", menuName = "Fishing/FishingRod")]
public class FishingRodObject : ScriptableObject
{
    public string rodName = "ph name";
    public string rodDescription = "ph description";

    public int strength;
    public int sizeModifier;
    public float speedModifier;
    public float minigameSpeed;
    public float price;
    public List<SlotsObject> slots;

    public Sprite rodImage;
}
