using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public FishObject fish;
    [SerializeField] private TextMeshPro fishName;
    [SerializeField] private TextMeshPro description;
    [SerializeField] private TextMeshPro weight;
    [SerializeField] private TextMeshPro size;
    [SerializeField] private TextMeshPro price;
    [SerializeField] private TextMeshPro difficulty;
    [SerializeField] private SpriteRenderer fishSpriteRenderer;
    [SerializeField] private SpriteRenderer playerFishSpriteRenderer;


    private void OnEnable()
    {
        
        SetAllVisuals();   
    }

    public void SetAllVisuals()
    {
        if (fish != null)
        {
            fish.modifiers.GenerateModifiers();

            fishName.text = fish.fishName;
            description.text = fish.description;
            weight.text = $"Weight: {fish.Weight:F1}kg";
            size.text = $"Size: {fish.Size:F1}m";
            price.text = $"Price: {fish.FishPrice:F1}G";
            difficulty.text = $"Difficulty: {fish.difficulty}";
            fishSpriteRenderer.sprite = fish.fishImage;
            playerFishSpriteRenderer.sprite = fish.fishImage;
            playerFishSpriteRenderer.transform.localScale = Vector3.one * fish.Size * 0.5f;
        }

    }
}
