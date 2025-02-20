using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FishingRod", menuName = "Fishing/FishingRod")]
public class FishingRodObject : ScriptableObject
{
    public int strength;
    public int sizeModifier;
    public float speedModifier;
    public float minigameSpeed;

    public Sprite rodImage;
}
