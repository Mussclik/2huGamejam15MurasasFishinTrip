using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string currentAnimation;
    public const string IDLE = "PlayerIdle";
    public const string IDLE_TO_FISHING = "PlayerIdleToFish";
    public const string IDLE_FISHING = "PlayerIdleFishing";
    public const string FISHING_TO_IDLE = "PlayerFishToIdle";
    public const string REELING_FISH = "PlayerFishReeling";
    public const string PULL_IN_FISH = "PlayerPullInFish";
    public const string SHOWING_FISH = "PlayerShowFishIdle";
    public const string SHOWING_FISH_TO_IDLE = "PlayerShowFishToIdle";

    public void PlayAnimation(string newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            animator.Play(newAnimation);
           
            currentAnimation = newAnimation;
        }
    }
}
