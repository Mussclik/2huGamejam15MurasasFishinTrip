using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGameSlot : MonoBehaviour
{

    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public SlotsObject slotInfo;

    public void ResetVisuals()
    {
        spriteRenderer.sprite = slotInfo.sprite;
    }


}

