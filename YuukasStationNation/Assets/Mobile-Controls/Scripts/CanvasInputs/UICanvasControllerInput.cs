using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    public PlayerMovement inputs;
    public Vector2 desiredDirection = Vector2.zero;
    public static UICanvasControllerInput instance;
    public bool buttonState = false;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        inputs = PlayerMovement.instance;
    }

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        desiredDirection = virtualMoveDirection;
    }

    //public void VirtualLookInput(Vector2 virtualLookDirection)
    //{
    //    inputs.look = virtualLookDirection;
    //}

    public void VirtualJumpInput(bool virtualJumpState)
    {
        if (virtualJumpState)
        {
            buttonState = true;
            Debug.Log("buttonisPressed");
        }
        else
        {
            buttonState = false;
        }
    }

    //public void VirtualSprintInput(bool virtualSprintState)
    //{
    //    inputs.sprint = virtualSprintState;
    //}

    //public void VirtualSwitchInput(bool virtualSwitchState)
    //{
    //    inputs.switchMode = virtualSwitchState;
    //}
}