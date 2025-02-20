using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingArea : MonoBehaviour
{
    [SerializeField] private List<FishObject> possibleFish;
    [SerializeField] private float maxModifierDevienceOfArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FishObject FishInArea(PlayerMovement player)
    {
        FishObject fishBeingCaught = Instantiate(possibleFish[Random.Range(0, possibleFish.Count)]);
        if(fishBeingCaught != null )
        {
            fishBeingCaught.modifiers.GenerateModifiers(0.1f, maxModifierDevienceOfArea);
        }

        if (player.equippedRod.strength >= fishBeingCaught.difficulty)
        {
            Debug.LogWarning("CaughtFish");
            fishBeingCaught.modifiers.TerrainDevience = maxModifierDevienceOfArea;
            return fishBeingCaught;
        }
        else
        {
            Debug.LogWarning("FishGotAway");
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("thing entered fish area");
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.currentFishingArea = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("thing left fish area");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * GetComponent<SphereCollider>().radius);
    }
}
