using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBaseClass : MonoBehaviour, Iinteractable
{
    private SpriteRenderer[] spriteRenderers;
    private bool playerIsHigher = false;

    protected PlayerMovement Player
    {
        get
        {
            return PlayerMovement.instance;
        }
    }
    
    protected virtual void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        CheckSpriteRenderLayers();
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
    protected void CheckSpriteRenderLayers()
    {
        if (transform.position.z > Player.transform.position.z && playerIsHigher)
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.sortingOrder -= 20;
            }
            playerIsHigher = false;
        }
        else if (transform.position.z < Player.transform.position.z && !playerIsHigher)
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.sortingOrder += 20;
            }
            playerIsHigher = true;
        }
    }

}
