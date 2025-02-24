using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreArea : AreaBaseClass
{
    public override void Interact()
    {
        StoreMenuScript.instance.gameObject.SetActive(true);
        GameManager.Gamestate = Gamestate.InIngameMenu;
    }
    protected override void OnPlayerEnter()
    {
        
    }
}
