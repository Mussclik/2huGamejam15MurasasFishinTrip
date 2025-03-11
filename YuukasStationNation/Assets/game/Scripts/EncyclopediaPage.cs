using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaPage : MonoBehaviour
{
    public int amountCaught = 0;
    public Biome biome;
    public int idOfFish;
    [SerializeField] private TextMeshProUGUI idAndNameText;
    [SerializeField] private TextMeshProUGUI refrenceText;
    [SerializeField] private TextMeshProUGUI zoneCaughtText;
    [SerializeField] private TextMeshProUGUI amountCaughtText;
    [SerializeField] private TextMeshProUGUI descriptionsText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI maxSizeText;
    [SerializeField] private TextMeshProUGUI maxWeightText;
    [SerializeField] private SpriteRenderer fishSprite;

    public float largestSize = 0f;
    public float largestWeight = 0f;

    public GameObject informationDisplayObject;

    private void OnEnable()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (idOfFishhcsd == -1) return;

        if (amountCaught > 0)
        {
            fishSprite.color = Color.black;
        }
        else //is unknown
        {
            fishSprite.color = Color.black;
        }
    }
    
    public bool RegisterFishCaught()
    {

    }
}

public enum Biome
{
    Normal,
    BloodWhells,
    Strange,
}