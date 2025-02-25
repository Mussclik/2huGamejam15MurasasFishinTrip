using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowArea : AreaBaseClass
{
    [SerializeField] private AudioClip crowNoise;
    [SerializeField] private float timeToCrow;
    [SerializeField] private Sprite crowNormal;
    [SerializeField] private Sprite crowActivated;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private TimerScript timer = new TimerScript();
    private TimerScript crowTimer = new TimerScript();
    public override void Interact()
    {
        if (timer.CompletionStatus)
        {
            Player.textspawner.CreateText("Those who crow");
            Player.ThrowMurasa();
            ChangeCrowState(crowActivated);
            timer.Start(timeToCrow);
            crowTimer.Start(crowNoise.length);
            SoundManager.instance.PlaySound(crowNoise,1, Random.Range(0.9f, 1.1f));
        }
        
    }
    protected override void Start()
    {
        base.Start();
        crowTimer.runOnFinish += () => ChangeCrowState(crowNormal);
    }
    protected override void OnPlayerEnter()
    {
        if (timer.CompletionStatus)
        {
            Player.textspawner.CreateText("Press F to Crow");
        }
        
    }
    protected override void Update()
    {
        base.Update();
        timer.Update();
        crowTimer.Update();
        
    }
    private void ChangeCrowState(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
