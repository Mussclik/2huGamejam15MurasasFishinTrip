using System;
using UnityEngine;

[Serializable]
public class GlobalTimer : IDisposable
{
    public float elapsedTime = 0;
    public float duration = 0;

    public bool isTimeScaled = false;
    public bool autoRestart = false;
    private bool firstUpdate = true;
    public bool logging = true;
    [SerializeField] private bool isRunning = false;


    public event System.Action callOnStart;
    public event System.Action callOnUpdate;
    public event System.Action callOnFinish;

    private ITimerLocalAttachable attachedScript;
    public GlobalTimer(float newDuration = 0)
    {
        ITimerStaticAttachable.onUpdateStatic += Update;
        duration = newDuration;
    }
    public GlobalTimer(ITimerLocalAttachable scriptToAttachTimer, float newDuration = 0)
    {
        attachedScript = scriptToAttachTimer;
        scriptToAttachTimer.onUpdateLocal += Update;
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
        Log("Timer, being updated call");
        if (isRunning)
        {
            //callOnStart check
            if (firstUpdate && callOnStart != null)
            {
                firstUpdate = false;
                if (callOnStart != null) callOnStart();
                Log("Timer, start call");
            }

            //update timer
            if (elapsedTime !>= duration)
            {
                if (!isTimeScaled) elapsedTime += Time.deltaTime;
                else elapsedTime += Time.deltaTime / Time.timeScale;

                if (callOnUpdate != null) callOnUpdate();
                Log("Timer, update call");
            }

            //finish check
            if (elapsedTime >= duration)
            {
                if (callOnFinish != null) callOnFinish();
                isRunning = false;
                if (autoRestart) Restart();
                Log("Timer, finish call");
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
        firstUpdate = true;
        isRunning = true;
    }
    public void Pause()
    {
        isRunning = false;
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

    public void Log(object message)
    {
        if (logging)
        {
            Debug.Log(message);
        }
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
            attachedScript.onUpdateLocal -= Update;
        }
        else
        {
            ITimerStaticAttachable.onUpdateStatic -= Update;
        }

    }
    #endregion
}