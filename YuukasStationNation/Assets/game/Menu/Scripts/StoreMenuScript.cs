using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenuScript : MonoBehaviour
{
    public static StoreMenuScript instance;

    [Header("Rod")]
    [SerializeField] private GameObject rodPrefab;
    [SerializeField] private Transform rodTab;
    [SerializeField] private Transform rodParent;
    private List<RodsListObject> rodList = new List<RodsListObject>();
    [SerializeField] private List<FishingRodObject> rods = new List<FishingRodObject>();

    [Header("Upgrade")]
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] private Transform upgradeTab;
    [SerializeField] private Transform upgradesParent;
    private List<UpgradesListObject> upgradeList = new List<UpgradesListObject>();
    [SerializeField] private List<UpgradeObject> upgrades;

    [Header("Fish")]
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform fishTab;
    [SerializeField] private Transform fishParent;
                     private List<FishListObject> fishList = new List<FishListObject>();
    [SerializeField] private List<FishObject> Fish
    {
        get { return PlayerMovement.instance.storedFish; }
    }

    [Header("Misc")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button rodsButton;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button fishButton;
    [SerializeField] private TextMeshProUGUI money;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        exitButton.onClick.AddListener(CloseStore);
        rodsButton.onClick.AddListener(() => ChangeSubMenu(rodTab));
        upgradesButton.onClick.AddListener(() => ChangeSubMenu(upgradeTab));
        fishButton.onClick.AddListener(() => ChangeSubMenu(fishTab));
        ResetAllObjects();
    }
    private void OnEnable()
    {
        //resetvisuals, resetfish, activate anytime a button is pressed
        ResetFish();
        ResetAllVisuals();
    }
    public void DestroyChildren<T>(Transform parentToUnparent, List<T> listToClear) // the code behind the slaughter
    {
        for (int i = 0; i < parentToUnparent.childCount; i++)
        {
            Destroy(parentToUnparent.GetChild(0).gameObject);
        }
        listToClear.Clear();
    }

    public void CreateRodsObjects()
    {
        for (int i = 0; i < rods.Count; i++)
        {
            RodsListObject newObject = Instantiate(rodPrefab, transform.position, Quaternion.identity, rodParent).GetComponent<RodsListObject>();
            rodList.Add(newObject);
            newObject.fishingRod = Instantiate(rods[i]);
            newObject.button.onClick.AddListener(ResetAllVisuals);
            Debug.Log(newObject.button);
            
        }
        ResetAllVisuals();
    }

    public void CreateUpgradeObjects()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradesListObject newObject = Instantiate(upgradePrefab, transform.position, Quaternion.identity, upgradesParent).GetComponent<UpgradesListObject>();
            upgradeList.Add(newObject);
            newObject.upgrade = Instantiate(upgrades[i]); // Assigns the correct upgrade object
            newObject.button.onClick.AddListener(ResetAllVisuals);
        }
        ResetAllVisuals();
    }

    public void CreateFishObjects()
    {
        if (Fish != null)
        {
            for (int i = 0; i < Fish.Count; i++)
            {
                FishListObject newObject = Instantiate(fishPrefab, transform.position, Quaternion.identity, fishParent).GetComponent<FishListObject>();
                fishList.Add(newObject);
                newObject.fish = Fish[i]; // Assigns the correct fish object
                newObject.button.onClick.AddListener(ResetAllVisuals);
            }
            ResetAllVisuals();
        }

    }

    public void ChangeSubMenu(Transform toEnable)
    {
        rodTab.gameObject.SetActive(false);
        upgradeTab.gameObject.SetActive(false);
        fishTab.gameObject.SetActive(false);
        toEnable.transform.transform.transform.transform.gameObject.SetActive(true);
        toEnable.GetComponentInChildren<Scrollbar>(true).value = 1;
    }

    public void ResetAllObjects()
    {
        DestroyChildren<RodsListObject>(rodParent, rodList);
        DestroyChildren<UpgradesListObject>(upgradesParent, upgradeList);
        DestroyChildren<FishListObject>(fishParent, fishList);

        CreateRodsObjects();
        CreateUpgradeObjects();
        CreateFishObjects();
    }
    public void ResetAllVisuals()
    {
        for(int i = 0; i < rodList.Count; i++)
        {
            if (rodList[i].fishingRod != PlayerMovement.instance.equippedRod)
            {
                rodList[i].isEquipped = false;
            }
            rodList[i].VisualUpdate();
        }
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeList[i].VisualUpdate();
        }
        for (int i = 0; i < fishList.Count; i++)
        {
            fishList[i].VisualUpdate();
        }
        money.text = $"{PlayerMovement.instance.money:F1}G";
    }
    public void CloseStore()
    {
        gameObject.SetActive(false);
        GameManager.Gamestate = Gamestate.MovingOnMap;
    }
    public void ResetFish()
    {
        DestroyChildren<FishListObject>(fishParent, fishList);
        CreateFishObjects();
    }
}
