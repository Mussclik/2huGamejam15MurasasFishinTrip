using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesListObject : ListObjectBase
{
    public UpgradeObject upgrade;
    public bool isOwned = false;
    public bool isEquipped = false;

    public string forOwned = "Equip";
    public string forEquipped = "Unequip";
    public string forUnowned = "Buy";

    public override void OnActivation()
    {
        if (isEquipped)
        {
            Player.equippedUpgrades.Remove(upgrade);
            isEquipped = false;
        }
        else if (isOwned)
        {
            Player.equippedUpgrades.Add(upgrade);
            isEquipped = true;
        }
        else if (Player.money >= upgrade.price)
        {
            Player.equippedUpgrades.Add(upgrade);
            isOwned = true;
            isEquipped = true;
        }
        VisualUpdate();
        StoreMenuScript.instance.ResetAllVisuals();
    }
    public override void VisualUpdate()
    {
        itemName.text = upgrade.upgradeName;
        description.text = upgrade.upgradeDescription + $" Cost: {upgrade.price}G";
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
