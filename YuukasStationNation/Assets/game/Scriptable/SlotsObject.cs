using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slot", menuName = "Slots/Slot Object")]
public class SlotsObject : ScriptableObject
{
    public int id = -999;
    public int singleMult = 1;
    public int doubleMult = 2;
    public int tripleMult = 6;
    public Sprite sprite;
}
