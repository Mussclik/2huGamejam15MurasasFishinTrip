using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //organize? nah, id put it in randomly when needed.
    public static PlayerMovement instance;

    [Header("Player Movement")]
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float drag = 1f;
    [SerializeField] private Vector2 currentVelocity = Vector2.zero;
    [SerializeField] private Vector2 desiredDirection = Vector2.zero;

    [SerializeField] private Rigidbody rigidBody;

    [Header("Boat Rotation Visuals")]
    
    [SerializeField] private float boatTiltX = 20f;
    [SerializeField] private float boatTiltY = 30f;
    [SerializeField] private float velocityThreshold = 6f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform boatVisuals;
    [Header("Animations")]

    public bool isAnimationFinished = false;

    [SerializeField] private PlayerAnimationHandler animator;

    public delegate void OnAnimationFinish();
    public OnAnimationFinish onAnimationFinish;

    [Header("Fishing")]
    public int maxFishStorage = 10;
    public float fishingSpeed;

    public FishingRodObject equippedRod;
    public FishingArea currentFishingArea;
    public FishObject fishOnHook;

    public List<FishObject> storedFish = new List<FishObject>();
    private TimerScript fishingTimer = new TimerScript();


    [Header("Misc")]

    [SerializeField] private RawImage gambleScreen;
    [SerializeField] private GameObject murasa;
    public AreaBaseClass currentInteractableArea;
    private TimerScript gambleFadeInTimer = new TimerScript(1);


    private void Awake()
    {
        bool shutup = false;
        if (shutup)
        {
            Debug.Log("I AM GOING INSANEEEE!!!! also hello!!! thanks for playing my game! " +
                "if your looking at the code of it, first off i am so sorry for your eyes. " +
                "second, you might aswell just ask me for access to the source github project on discord. " +
                "my username is just mussclik. anyway, gatekeep, girbloss and slay on bestie! :3");
        }
        instance = this;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        //rigidBody.drag = drag; // Use Unity's built-in drag for smooth deceleration
    }

    // Start is called before the first frame update
    void Update()
    {
        desiredDirection = Vector2.zero;
        if (GameManager.Gamestate == Gamestate.MovingOnMap)
        {
            MovementCheck();
        }
        FishCheck();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ObjectLauncher objectLauncher = new ObjectLauncher();
            GameObject newMurasa = objectLauncher.LaunchObject(murasa);
            newMurasa.transform.localScale = murasa.transform.lossyScale;
            Destroy(newMurasa, 3);


        }
    }

    private void FixedUpdate()
    {
        if (desiredDirection != Vector2.zero)
        {
            Vector3 force = new Vector3(desiredDirection.x, 0, desiredDirection.y) * acceleration;
            rigidBody.AddForce(force, ForceMode.Acceleration);
            rigidBody.drag = 0.10f;
        }
        else
        {
            rigidBody.drag = drag;
        }

        // Clamp speed to maxSpeed
        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }

        RotateBoatVisuals();
    }

    private void MovementCheck()
    {
        
        if (Input.GetKey(KeyCode.W)) desiredDirection.y += 1;
        if (Input.GetKey(KeyCode.S)) desiredDirection.y -= 1;
        if (Input.GetKey(KeyCode.D)) desiredDirection.x += 1;
        if (Input.GetKey(KeyCode.A)) desiredDirection.x -= 1;
        desiredDirection.Normalize();
    }

    void RotateBoatVisuals()
    {
        if (!boatVisuals || !rigidBody) return; // Prevent errors if boatVisuals or rigidBody is not assigned

        Vector2 velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.z); // Convert Rigidbody velocity to 2D

        float targetXRotation = 0;
        float targetYRotation = 0;

        // Handle X tilt (left/right movement)
        if (velocity.x > 0)
        {
            targetXRotation = -boatTiltX;
            targetYRotation = 180; // Facing right
        }
        else if (velocity.x <= 0)
        {
            targetXRotation = boatTiltX;
            targetYRotation = 0; // Facing left
        }

        // Handle Y tilt (forward/backward movement), relative to boat direction
        if (Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x) * 0.45f && Mathf.Abs(velocity.x) > 2) // If moving more forward/backward than sideways
        {
            if (velocity.y > 0) // Moving forward
            {
                targetYRotation += (velocity.x >= 0) ? -boatTiltY : boatTiltY;
            }
            else if (velocity.y < 0) // Moving backward
            {
                targetYRotation += (velocity.x > 0) ? boatTiltY : -boatTiltY;
            }
        }

        // Smoothly interpolate rotation
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);
        boatVisuals.rotation = Quaternion.Lerp(boatVisuals.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FishCheck() //i heart coroutines
    {
        fishingTimer.Update();
        gambleFadeInTimer.Update();


        //start fishing
        if (Input.GetKeyDown(KeyCode.F) && currentFishingArea != null && GameManager.Gamestate == Gamestate.MovingOnMap)
        {
            StartCoroutine(StartFishing());
        }



        //cancelfishing
        else if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape)) && GameManager.Gamestate == Gamestate.Fishing)
        {
            StartCoroutine(FinishFishing());
        }



        //start reeling fish
        if (fishingTimer.CompletionStatus && GameManager.Gamestate == Gamestate.Fishing)
        {
            fishOnHook = currentFishingArea.FishInArea(this);
            if (fishOnHook != null)
            {
                StartCoroutine(StartReeling());
            }
            else
            {
                StartCoroutine(FinishFishing());
            }
        }

        //stop looking at fish, move again on map
        if (Input.GetKeyDown(KeyCode.F) && GameManager.Gamestate == Gamestate.LookingAtFishCaught)
        {
            StartCoroutine(ShowingFishToIdle());
        }

    }

    private void InteractCheck()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentInteractableArea != null)
        {
            currentInteractableArea.Interact();
        }
    }

    #region Coroutines 
    //JAG... ÄLSKAR... COROUTINES!!!!
    private IEnumerator StartFishing()
    {
        AnimationSetup();
        animator.PlayAnimation(PlayerAnimationHandler.IDLE_TO_FISHING); //start transitionToFishing animation

        while (!isAnimationFinished)
        {
            yield return null;
        }

      //on animation finish, start timer and idle fishing animation
        fishingTimer.enabled = true;
        fishingTimer.Start(fishingSpeed * equippedRod.speedModifier * UnityEngine.Random.Range(0.7f, 1.3f));
        animator.PlayAnimation(PlayerAnimationHandler.IDLE_FISHING); //start idle fishing animation
        GameManager.Gamestate = Gamestate.Fishing;

        
    }

    public IEnumerator FinishFishing()
    {
        bool slotsVisible;
        AnimationSetup();
        animator.PlayAnimation(PlayerAnimationHandler.FISHING_TO_IDLE); //start transitionToFishing animation
        SoundManager.instance.PlayMusic(1);
        Color color = Color.white;

        if (gambleScreen.color.a != 0)
        {
            slotsVisible = true;
            gambleFadeInTimer.Restart();
        }
        else
        {
            slotsVisible = false;
        }

        //end fishing timer
        fishingTimer.enabled = false;

        //begin reeling in animation

        while (!isAnimationFinished)
        {
            if (slotsVisible)
            {
                color = gambleScreen.color;
                color.a = 1 - gambleFadeInTimer.Progress();
                gambleScreen.color = color;
            }
            yield return null;
        }

        color.a = 0;
        gambleScreen.color = color;
        GameManager.Gamestate = Gamestate.MovingOnMap;
        animator.PlayAnimation(PlayerAnimationHandler.IDLE);

    }

    private IEnumerator StartReeling()
    {

        SlotGameHandler.instance.gameObject.SetActive(false);
        SlotGameHandler.instance.gameObject.SetActive(true);
        //just turn it off and on again smh, works everytime


        AnimationSetup();
        gambleScreen.gameObject.SetActive(true);
        Color color = gambleScreen.color;
        color.a = 0;
        gambleScreen.color = color;

        SoundManager.instance.PlayMusic(2);
        gambleFadeInTimer.Restart();
        isAnimationFinished = false;
        GameManager.Gamestate = Gamestate.InAnimation;
        SlotGameHandler.instance.pressToStartText.gameObject.SetActive(false);
        animator.PlayAnimation(PlayerAnimationHandler.REELING_FISH);


        //start fading in gambling screen animation
        //begin ultra epic fishing music

        while (!gambleFadeInTimer.CompletionStatus)
        {
            color.a = gambleFadeInTimer.Progress();
            gambleScreen.color = color;
            yield return null;
        }

        color.a = 1;
        gambleScreen.color = color;
      //on animation finish, start reeling idle animation
        GameManager.Gamestate = Gamestate.ActivelyCatchingFish;
        SlotGameHandler.instance.pressToStartText.gameObject.SetActive(true);
        //enable gambling minigame
        //enable

    }

    public IEnumerator ReelInFish()
    {
        AnimationSetup();

        animator.PlayAnimation(PlayerAnimationHandler.PULL_IN_FISH);
        SoundManager.instance.PlayMusic(1);
        //reel to hold fish idle
        while (!isAnimationFinished)
        {
            yield return null;
        }
        animator.PlayAnimation(PlayerAnimationHandler.SHOWING_FISH);
        GameManager.Gamestate = Gamestate.LookingAtFishCaught;
    }

    private IEnumerator ShowingFishToIdle()
    {
        AnimationSetup();

        animator.PlayAnimation(PlayerAnimationHandler.SHOWING_FISH_TO_IDLE);

        Color color = gambleScreen.color;
        color.a = 0;
        gambleScreen.color = color;
        gambleFadeInTimer.Restart();
        //hold fish idle to normal idle
        while (!isAnimationFinished)
        {
            color = gambleScreen.color;
            color.a = 1 - gambleFadeInTimer.Progress();
            gambleScreen.color = color;
            yield return null;
        }

        color.a = 0;
        gambleScreen.color = color;

        animator.PlayAnimation(PlayerAnimationHandler.IDLE);
        GameManager.Gamestate = Gamestate.MovingOnMap;
    }

    private void AnimationSetup()
    {
        isAnimationFinished = false;
        GameManager.Gamestate = Gamestate.InAnimation;
    }

    public void FinishAnimation()
    {
        isAnimationFinished = true;

        if (onAnimationFinish != null)
        {
            onAnimationFinish();
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), new Vector3(desiredDirection.x, 0, desiredDirection.y) * 3);
    }

}

