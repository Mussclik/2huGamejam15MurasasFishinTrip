using System;
using UnityEngine;

[Serializable]
public class GlobalTimer : IDisposable
{
    public float elapsedTime = 0;
    public float duration = 0;

    public bool isTimeScaled = false;
    public bool autoRestart = false;
    [SerializeField] private bool isRunning = false;


    public System.Action callOnStart;
    public System.Action callOnUpdate;
    public System.Action callOnFinish;

    private ITimerLocalAttachable attachedScript;
    public GlobalTimer(float newDuration = 0)
    {
        ITimerStaticAttachable.onUpdate += Update;
        duration = newDuration;
    }
    public GlobalTimer(ITimerLocalAttachable scriptToAttachTimer, float newDuration = 0) 
    {
        attachedScript = scriptToAttachTimer;
        scriptToAttachTimer.onUpdate += Update;
        duration = newDuration;
    }


    public bool IsFinished
    {
        get 
        {
            return elapsedTime >= duration;
        }
    }

    /// <summary>
    /// Returns a value from 0.00f to 1.00f
    /// </summary>
    public float PercentFinished
    {
        get
        {
            float percent = elapsedTime / duration;
            if (percent > 1) percent = 1;

            return Mathf.Round(percent * 100) * 0.01f; 
        }
    }

    public void Update()
    {
        if (isRunning)
        {
            //callOnStart check
            if (elapsedTime >= 0.001f && callOnStart != null) callOnStart();

            //update timer
            if (!isTimeScaled) elapsedTime += Time.deltaTime;
            else elapsedTime += Time.deltaTime / Time.timeScale;
            
            if(callOnUpdate != null) callOnUpdate();

            //finish check
            if (elapsedTime >= duration)
            {
                if (callOnFinish != null) callOnFinish();

                if (autoRestart) Restart();
                else isRunning = false;
            }
        }
    }
    public void Start(float newDuration)
    {
        duration = newDuration;
        Restart();
    }
    public void Restart()
    {
        elapsedTime = 0f;
        
        isRunning = true;
    }
    public void Pause()
    {
        isRunning=false;
    }
    public void Resume()
    {
        isRunning = true;
    }

    /// <summary>
    /// adds to elapsed time by the amount given. if changePercent, elapsed + (amount * duration)
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="changePercent"></param>
    public void ChangeElapsedTime(float amount, bool changePercent = true)
    {
        if (changePercent) elapsedTime += duration * amount;
        else elapsedTime += amount;
    }

    protected virtual void VirtualStart()
    {

    }

    #region disposer
    ~GlobalTimer() // EVIL constructor
    {
        Dispose();
    }
    public void Dispose()
    {
        if (attachedScript != null)
        {
            attachedScript.onUpdate -= Update;
        }
        else
        {
            ITimerStaticAttachable.onUpdate -= Update;
        }
        
    }
    #endregion
}