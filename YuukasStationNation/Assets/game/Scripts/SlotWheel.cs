using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotWheel : MonoBehaviour
{
    [SerializeField] public List<SlotGameSlot> slots = new List<SlotGameSlot>();

    [SerializeField] private List<Vector3> startPositions = new List<Vector3>();
    private Vector3 bottomPos;
    private Vector3 topPos;
    public Transform ArrowPosition;
    [SerializeField] private bool isActive;
    private float topToBottomDistance = 0;
    public bool hasFinished = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        hasFinished = false;
        if (startPositions.Count != slots.Count)
        {
            startPositions = new Vector3[slots.Count].ToList();
            for(int i = 0; i < slots.Count; i++)
            {
                startPositions[i] = slots[i].transform.position;
                slots[i].gameObject.SetActive(false);
                slots[i].slotInfo = SlotGameHandler.instance.RollNewSlot();
                slots[i].ResetVisuals();
                slots[i].spriteRenderer.color = Color.white;
            }
            topPos = startPositions[0];
            bottomPos = startPositions[startPositions.Count - 1] - new Vector3(0, Vector3.Distance(startPositions[startPositions.Count - 1], startPositions[startPositions.Count - 2]),0);
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].transform.position = startPositions[i];
                slots[i].gameObject.SetActive(false);
                slots[i].slotInfo = SlotGameHandler.instance.RollNewSlot();
                slots[i].ResetVisuals();
                slots[i].spriteRenderer.color = Color.white;
            }
        }
        topToBottomDistance = Vector3.Distance(topPos, bottomPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            foreach(SlotGameSlot slot in slots)
            {
                slot.transform.position = new Vector3(
                    slot.transform.position.x,
                    slot.transform.position.y - topToBottomDistance * Time.deltaTime * SlotGameHandler.instance.WheelSpeed,
                    slot.transform.position.z);

                if (slot.transform.position.y < bottomPos.y)
                {
                    ResetSlotPositionAndState(slot);
                }
            }

        }
    }

    public SlotsObject GetClosestWinner()
    {
        float smallestDistance = 9999999f;
        SlotGameSlot closest = null;
        for (int i = 0; i < slots.Count; i++)
        {
            float distance = Vector3.Distance(slots[i].transform.position, Vector3.Lerp(topPos, startPositions[startPositions.Count - 1], 0.5f));
            if (distance < smallestDistance)
            {
                closest = slots[i];
                smallestDistance = distance;
            }
        }
        Debug.Log(closest.gameObject);
        return closest.slotInfo;
    }
    public SlotsObject GetClosestWinner(out SlotGameSlot closestsgs)
    {
        float smallestDistance = 9999999f;
        SlotGameSlot closest = null;
        for (int i = 0; i < slots.Count; i++)
        {
            float distance = Vector3.Distance(slots[i].transform.position, Vector3.Lerp(topPos, startPositions[startPositions.Count - 1], 0.5f));
            if (distance < smallestDistance)
            {
                closest = slots[i];
                smallestDistance = distance;
            }
        }
        closestsgs = closest;
        return closest.slotInfo;
    }

    public void Activate()
    {
        foreach (SlotGameSlot slot in slots)
        {
            slot.gameObject.SetActive(true);
            
        }
        isActive = true;
    }
    public void Deactivate()
    {
        isActive = false;
        hasFinished = true;
        SlotsObject slotsObject = GetClosestWinner(out SlotGameSlot closestObject);
        foreach (SlotGameSlot slot in slots)
        {
            slot.spriteRenderer.color = Color.red;
        }
        closestObject.spriteRenderer.color = Color.white;
    }
    public void ResetSlotPositionAndState(SlotGameSlot slotToAdjust)
    {
        Vector3 difference = slotToAdjust.transform.position - bottomPos;
        slotToAdjust.transform.position = topPos + difference;
        slotToAdjust.slotInfo = SlotGameHandler.instance.RollNewSlot();
        slotToAdjust.ResetVisuals();
    }
}
