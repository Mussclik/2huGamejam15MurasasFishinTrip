using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class FishListObject : ListObjectBase
{
    public delegate void OnChange();
    public static OnChange onChange;
    public TextMeshProUGUI size;
    public TextMeshProUGUI prize;
    public TextMeshProUGUI weight;
    public TextMeshProUGUI difficulty;

    public FishObject fish;

    public override void OnActivation()
    {
        Player.storedFish.Remove(fish);
        Destroy(gameObject);
        Player.money += fish.FishPrice;
        StoreMenuScript.instance.ResetAllVisuals();
    }
    public override void VisualUpdate()
    {
        itemName.text = fish.fishName;
        description.text = fish.description;
        spriteRenderer.sprite = fish.fishImage;
        size.text = $"{fish.Size:F1}cm";
        prize.text = $"{fish.FishPrice:F1}G";
        weight.text = $"{fish.Weight:F1}kg";
        difficulty.text = $"Difficulty: {fish.difficulty}";
        buttonText.text = "sell";
        
    }
    public void UpdateButtonText()
    {

        buttonText.text = $"{fish.FishPrice}G";
        
    }
}
