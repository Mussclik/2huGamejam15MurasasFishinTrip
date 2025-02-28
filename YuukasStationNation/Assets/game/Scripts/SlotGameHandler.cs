using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SlotGameHandler : MonoBehaviour
{
    public static SlotGameHandler instance;
    [SerializeField] private float wheelSpeed;
    public float WheelSpeed
    {
        get { return wheelSpeed * PlayerMovement.instance.equippedRod.minigameSpeed; }
    }

    public List<SlotsObject> slots = new List<SlotsObject>();
    [SerializeField] private List<SlotsObject> currentSlotPool = new List<SlotsObject>();

    public TimerScript timer = new TimerScript(10);
    public Transform timerMask;
    public Transform timerTop;
    public Transform timerBottom;
    public Transform arrowPointer;
    public List<SlotWheel> wheels = new List<SlotWheel>();
    public GamblingState gamblingState;
    public float timeToGamble;
    [SerializeField] private int currentWheelSelected = -1;

    [SerializeField] private TextMeshPro multText;
    [SerializeField] public TextMeshPro pressToStartText;
    private int currentMult;

    [SerializeField] private GameObject slotsDisplay;
    [SerializeField] private FishDisplay fishDisplay;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer.runOnFinish += () => FinishMinigame(false); //listen... i wouldnt question it... lambda expression stuff... it just works...
    }
    private void OnEnable()
    {

        ResetSlotGame();
        slots = PlayerMovement.instance.equippedRod.slots;
        gamblingState = GamblingState.NotStarted;
        currentWheelSelected = -1;
        arrowPointer.gameObject.SetActive(false);
        pressToStartText.gameObject.SetActive(true);
        timerMask.position = timerTop.position;
        slotsDisplay.SetActive(true);
        fishDisplay.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Gamestate == Gamestate.ActivelyCatchingFish && Input.GetKeyDown(KeyCode.F))
        {

            if (gamblingState == GamblingState.NotStarted)
            {

                StartMinigame();

            }
            else if (gamblingState == GamblingState.Gambling)
            {
                
                ProgressWheelSpinning();
            }
            else if (gamblingState == GamblingState.Finished)
            {
                FinishMinigame(true);
            }
        }

        if (gamblingState == GamblingState.Gambling)
        {
            timer.Update();
            timerMask.position = Vector3.Lerp(timerTop.position, timerBottom.position, timer.Progress());
        }
    }

    private void ProgressWheelSpinning() //start, go to next or end wheel spinning operations
    {
        if (currentWheelSelected < wheels.Count && currentWheelSelected >= 0)
        {
            wheels[currentWheelSelected].Deactivate();
        }

        currentWheelSelected++;

        if (currentWheelSelected < wheels.Count)
        {
            wheels[currentWheelSelected].Activate();
            arrowPointer.transform.position = wheels[currentWheelSelected].ArrowPosition.position;
        }
        else
        {
            PlayingFinished();
        }

        CalculateMultiplier();
        Debug.Log("progresswheelspinning run");
    }

    private void CalculateMultiplier()
    {
        currentMult = 1;
        Dictionary<int, int> idCount = new Dictionary<int, int>(); // ID -> Count mapping

        // Step 1: Count occurrences of each unique slot ID
        foreach (var wheel in wheels)
        {
            if (wheel.hasFinished)
            {
                int id = wheel.GetClosestWinner().id;

                if (idCount.ContainsKey(id))
                    idCount[id]++;
                else
                    idCount[id] = 1;
            }
        }

        //Debug.Log($"ID Count Mapping: {string.Join(", ", idCount.Select(kvp => $"ID {kvp.Key}: {kvp.Value}x"))}");

        // Step 2: Apply multipliers based on the occurrences of each ID
        foreach (var kvp in idCount)
        {
            int id = kvp.Key;
            int count = kvp.Value;

            // Find the winning slot object with this ID
            SlotsObject slot = wheels.First(w => w.GetClosestWinner().id == id).GetClosestWinner();

            //Debug.Log($"Processing ID {id}, Count {count}, SingleMult {slot.singleMult}, DoubleMult {slot.doubleMult}, TripleMult {slot.tripleMult}");

            // Apply the correct multiplier
            if (count == 1)
                currentMult *= slot.singleMult;
            else if (count == 2)
                currentMult *= slot.doubleMult;
            else if (count == 3)
                currentMult *= slot.tripleMult;
            else if (count >= 4)
            {
                // Exponential growth for 4+ matches (adjust as needed)
                currentMult *= (int)Mathf.Pow(slot.tripleMult, count - 2);
                //Debug.Log($"Applying exponential scaling for ID {id}: New Multiplier = {currentMult}");
            }
        }

        //Debug.Log($"Final Multiplier: {currentMult}");
        multText.text = $"{currentMult}x";
    }


    public class MultThing
    {
        public SlotsObject slotObject;
        public int count;
    }

    public SlotsObject RollNewSlot()
    {
        if (currentSlotPool.Count < 1)
        {
            ResetSlotPool();
        }

        int randomNumber = UnityEngine.Random.Range(0, currentSlotPool.Count);
        SlotsObject slotToReturn = Instantiate(currentSlotPool[randomNumber]);
        currentSlotPool.RemoveAt(randomNumber);
        return slotToReturn;
        
    }

    private void ResetSlotGame()
    {
        timer.enabled = false;
        timerMask.position = timerTop.position;
        ResetSlotPool();
    }

    public void StartMinigame()
    {
        timer.enabled = true;
        timer.Start(timeToGamble);
        gamblingState = GamblingState.Gambling;
        pressToStartText.gameObject.SetActive(false);
        arrowPointer.gameObject.SetActive(true);
        ProgressWheelSpinning();

        //disable text saying press F to start
    }
    public void PlayingFinished()
    {
        timer.enabled = false;
        gamblingState = GamblingState.Finished;
        arrowPointer.gameObject.SetActive(false);
    }

    public void FinishMinigame(bool isSuccesful)
    {
        if (!isSuccesful)
        {
            StartCoroutine(PlayerMovement.instance.FinishFishing());
            PlayerMovement.instance.textspawner.CreateText("MASSIVE Skillissue!");
            PlayerMovement.instance.ThrowMurasa();
        }
        else
        {
            PlayerMovement.instance.fishOnHook.modifiers.slotsMultiplier = currentMult;
            StartCoroutine(PlayerMovement.instance.ReelInFish());
            //play animation for player to hold fish and display
            //change gamblescreen to fishdisplay
            fishDisplay.fish = PlayerMovement.instance.fishOnHook;
            fishDisplay.gameObject.SetActive(true);
            slotsDisplay.SetActive(false);
        }
    }

    public void ResetSlotPool()
    {
        currentSlotPool.Clear();
        foreach (var slot in slots)
        {
            currentSlotPool.Add(slot);
            currentSlotPool.Add(slot);
            currentSlotPool.Add(slot);
        }
    }


}

public enum GamblingState
{
    NotStarted,
    Gambling,
    Finished,
}