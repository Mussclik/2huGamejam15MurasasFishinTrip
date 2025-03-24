using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingArea : AreaBaseClass
{
    //[SerializeField] private List<FishObject> possibleFish;
    [SerializeField] private float maxModifierDevienceOfArea;
    [SerializeField] private Biome biome;

    public bool effectPlayerY;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public FishObject FishInArea(PlayerMovement player)
    {
        //FishObject fishBeingCaught = Instantiate(possibleFish[Random.Range(0, possibleFish.Count)]);

        FishObject fishBeingCaught = GameManager.instance.GetFishByBiome(biome, Player.FishingStrength);
        if (fishBeingCaught != null)
        {
            fishBeingCaught.modifiers.GenerateModifiers(0.1f, maxModifierDevienceOfArea);
        }

        if (player.FishingStrength >= fishBeingCaught.difficulty)
        {
            Debug.LogWarning("CaughtFish");
            fishBeingCaught.modifiers.TerrainDevience = maxModifierDevienceOfArea;
            fishBeingCaught.baseFishPrice *= Player.PriceModifier;
            return fishBeingCaught;
        }
        else return null;
    }

    protected override void OnPlayerEnter()
    {
        Player.currentFishingArea = this;
    }
    protected override void OnPlayerExit()
    {
        Player.currentFishingArea = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * GetComponent<SphereCollider>().radius);
    }


}
