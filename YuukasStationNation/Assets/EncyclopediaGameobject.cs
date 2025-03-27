using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaGameobject : MonoBehaviour
{
    [SerializeField] private EncyclopediaPage currentPage;
    [SerializeField] private int currentID;

    [SerializeField] private TextMeshProUGUI idAndNameText;
    [SerializeField] private TextMeshProUGUI refrenceText;
    [SerializeField] private TextMeshProUGUI zoneCaughtText;
    [SerializeField] private TextMeshProUGUI amountCaughtText;
    [SerializeField] private TextMeshProUGUI descriptionsText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI maxSizeText;
    [SerializeField] private TextMeshProUGUI maxWeightText;
    [SerializeField] private Image fishSprite;
    [SerializeField] private GameObject fishStatisticInformation;
    [SerializeField] public GameObject everythingInfo;

    private void OnEnable()
    {
        RefreshPage();
    }

    private void RefreshPage()
    {
        UpdateWithNewPage(currentPage);
    }

    public void UpdateWithNewPage(EncyclopediaPage page)
    {
        currentPage = page;

        if (page.amountCaught > 0)
        {
            idAndNameText.text = $"#{page.fishID} - {page.Fish.fishName}"; 
            descriptionsText.text = page.Fish.description;
            refrenceText.text = $"Reference: {page.Fish.refrence}";
            zoneCaughtText.text = page.Fish.biome.ToString();
            amountCaughtText.text = $"Caught: {page.amountCaught}";
            difficultyText.text = $"Skill lvl: {page.Fish.difficulty}";
            maxSizeText.text = $"{page.largestSize}m";
            maxWeightText.text = $"{page.largestWeight}kg";
            
            fishSprite.sprite = page.Fish.fishImage;
            fishSprite.color = Color.white;

            fishStatisticInformation.SetActive(true);
        }
        else
        {
            idAndNameText.text = $"#{page.fishID} - ???";
            difficultyText.text = $"Skill lvl: {page.Fish.difficulty}";
            zoneCaughtText.text = page.Fish.biome.ToString();

            fishSprite.sprite = page.Fish.fishImage;
            fishSprite.color = Color.black;

            fishStatisticInformation.SetActive(false);
        }
    }
}


