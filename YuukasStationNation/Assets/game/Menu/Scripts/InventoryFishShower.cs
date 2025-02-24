using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFishShower : MonoBehaviour
{
    public GameObject FishObjectPrefab;
    public Transform objectParent;
    public List<FishListObject> fishListObjects = new List<FishListObject>();

    private void OnEnable()
    {
        OnChange();   
    }
    public void OnChange()
    {
        DestroyChildren(objectParent);
        CreateFishObjects();
    }

    public void DestroyChildren(Transform parentToUnparent) // the code behind the slaughter
    {
        for (int i = 0; i < parentToUnparent.childCount; i++)
        {
            Destroy(parentToUnparent.GetChild(0).gameObject);
        }
        fishListObjects.Clear();
    }

    public void CreateFishObjects()
    {
        for (int i = 0; i < PlayerMovement.instance.storedFish.Count; i++)
        {
            fishListObjects.Add(Instantiate(FishObjectPrefab, transform.position, Quaternion.identity, objectParent).GetComponent<FishListObject>());
            fishListObjects[i].fish = PlayerMovement.instance.storedFish[i];
            fishListObjects[i].VisualUpdate();
        }
    }

}
