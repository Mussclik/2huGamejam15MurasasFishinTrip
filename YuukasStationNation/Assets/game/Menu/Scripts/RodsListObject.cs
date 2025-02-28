using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RodsListObject : ListObjectBase
{
    [SerializeField] public FishingRodObject fishingRod;
    public bool isOwned;
    public bool isEquipped;
    public string forOwned = "Equip";
    public string forEquipped = "Equipped";
    public string forUnowned = "Buy";


    public override void OnActivation()
    {

        if (isEquipped)
        {

        }
        else if (isOwned)
        {
            Player.equippedRod = fishingRod;
            isEquipped = true;
        }
        else if (Player.money >= fishingRod.price)
        {
            Player.equippedRod = fishingRod;
            isOwned = true;
            isEquipped = true;
            Player.money -= fishingRod.price;
        }
        VisualUpdate();
        StoreMenuScript.instance.ResetAllVisuals();
    }

    public override void VisualUpdate()
    {
        itemName.text = fishingRod.rodName;
        description.text = fishingRod.rodDescription + $" Cost: {fishingRod.price}G";
        if (isEquipped)
        {
            buttonText.text = forEquipped;
        }
        else if (isOwned)
        {
            buttonText.text = forOwned;
        }
        else
        {
            buttonText.text = forUnowned;
        }
    }

}
