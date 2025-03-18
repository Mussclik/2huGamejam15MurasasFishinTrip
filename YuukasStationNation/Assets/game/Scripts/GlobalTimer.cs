using System;
using UnityEditor;
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

    public GlobalTimer(float newDuration = 0) 
    {
        GameManager.onUpdate += Update;
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

    #region disposer
    ~GlobalTimer() //EVIL constructor, also known as deconstructor
    {
        Dispose();
    }
    public void Dispose()
    {
        GameManager.onUpdate -= Update;
    }
    #endregion
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(GlobalTimer))]
public class GlobalTimer_Editor : DecoratorDrawer
{
    private bool editorDropdown = false;

    public override void OnGUI(Rect position)
    {
        // Adjust the position for the foldout
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        editorDropdown = EditorGUI.Foldout(foldoutRect, editorDropdown, "Debug Options");

        if (editorDropdown)
        {
            // Adjust the position for the button
            Rect buttonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.5f, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Press Button"))
            {
                // Button functionality
                Debug.Log("Button Pressed!");
            }
        }
    }

    public override float GetHeight()
    {
        // Calculate the height based on whether the foldout is open
        if (editorDropdown)
        {
            return EditorGUIUtility.singleLineHeight * 2; // Foldout + button
        }
        return EditorGUIUtility.singleLineHeight; // Just the foldout
    }
}
#endif
