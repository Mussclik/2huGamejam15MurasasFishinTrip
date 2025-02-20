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
            weight.text = $"{fish.Weight}kg";
            size.text = $"{fish.Size}m";
            fishSpriteRenderer.sprite = fish.fishImage;
            playerFishSpriteRenderer.sprite = fish.fishImage;
            playerFishSpriteRenderer.transform.localScale = Vector3.one * fish.Size * 0.5f;
        }

    }
}
