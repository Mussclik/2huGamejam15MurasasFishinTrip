using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBaseClass : MonoBehaviour, Iinteractable
{
    protected PlayerMovement Player
    {
        get
        {
            return PlayerMovement.instance;
        }
    }
    
    public virtual void Interact()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("thing entered fish area");
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.currentInteractableArea = this;
            OnPlayerEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("thing entered fish area");
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            if (player.currentInteractableArea == this)
            {
                player.currentInteractableArea = null;
            }
            OnPlayerExit();
            
        }
    }
    protected virtual void OnPlayerEnter()
    {
        Player.textspawner.CreateText("Press F to Interact");
    }
    protected virtual void OnPlayerExit()
    {

    }

}
