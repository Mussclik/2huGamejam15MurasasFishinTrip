using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaGameobject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI idAndNameText;
    [SerializeField] private TextMeshProUGUI refrenceText;
    [SerializeField] private TextMeshProUGUI zoneCaughtText;
    [SerializeField] private TextMeshProUGUI amountCaughtText;
    [SerializeField] private TextMeshProUGUI descriptionsText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI maxSizeText;
    [SerializeField] private TextMeshProUGUI maxWeightText;
    [SerializeField] private SpriteRenderer fishSprite;
    [SerializeField] private GameObject visualObject;
    
    public void UpdatePage(EncyclopediaPage page, int id = -99999)
    {
        if (page.amountCaught > 0)
        {
            idAndNameText.text = $"#{id} - {page.fish.fishName}"; 
            descriptionsText.text = page.fish.description;
            refrenceText.text = $"Reference: {page.fish.Refrence}";
            zoneCaughtText.text = page.biome.ToString();
            amountCaughtText.text = $"Caught: {page.amountCaught}";
            difficultyText.text = $"Skill lvl: {page.fish.difficulty}";
            maxSizeText.text = $"{page.largestSize}m";
            maxWeightText.text = $"{page.largestWeight}kg";
            
            fishSprite.sprite = page.fish.fishImage;
            fishSprite.color = Color.white;

            visualObject.SetActive(true);
        }
        else
        {
            idAndNameText.text = $"#{id} - ???";
            difficultyText.text = $"Skill lvl: {page.fish.difficulty}";
            
            fishSprite.sprite = page.fish.fishImage;
            fishSprite.color = Color.black;

            visualObject.SetActive(false);
        }
    }
}
