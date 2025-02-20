using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimerScript
{
    [SerializeField] protected float timer = 0;
    [SerializeField] protected float duration;
    [SerializeField] public bool enabled = true;
    [SerializeField] public bool timeIsScaled = false;
    protected bool finished = true;

    //delegate
    public delegate void Deligate();
    public Deligate runOnFinish;
    public Deligate runOnUpdate;

    internal TimerScript()
    {
        this.duration = 0;
    }
    internal TimerScript(float duration)
    {
        this.duration = duration;
    }

    #region GETSET
    public float elapsedTime
    {
        get
        {
            return timer;
        }
    }
    public float Duration
    {
        get
        {
            return duration;
        }
    }
    public bool CompletionStatus
    {
        get
        {
            return Check();
        }
    }
    #endregion

    protected bool Check()
    {

        if (timer >= duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Restart()
    {
        timer = 0;
        finished = false;
    }
    public void Start(float duration)
    {
        timer = 0;
        this.duration = duration;
        finished = false;
    }
    public virtual void Update()
    {
        if (enabled)
        {
            //if done time is less than full duration
            if (timer < duration)
            {
                if (runOnUpdate != null)
                {
                    runOnUpdate();
                }
            }
            //if done time is above full duration and finished flag is not true
            else if (timer > duration && !finished)
            {
                finished = true;
                if (runOnFinish != null)
                {
                    runOnFinish();
                }
            }

            if (!timeIsScaled)
            {
                timer += Time.deltaTime;
            }
            else
            {
                float scaledtime = 0;
                if (Time.timeScale != 0)
                {
                    scaledtime = Time.deltaTime / Time.timeScale;
                }
                timer += scaledtime;
            }
        }
    }
    public float Progress()
    {
        return (timer / duration);
    }
}