using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimerStaticAttachable
{
    public static System.Action onUpdateStatic
    {
        get 
        { 
            if (refrenceScript != null) return refrenceScript.UpdateTimer;
            else return null;
        }
        set 
        {
            if (refrenceScript != null) refrenceScript.UpdateTimer = value;
            else return;  
        }
    }
    private static ITimerStaticAttachable refrenceScript;
    public static void AttachUpdatingScript(ITimerStaticAttachable scriptToAttach)
    {
        refrenceScript = scriptToAttach;
    }
    public System.Action UpdateTimer { get; set; }
}
public interface ITimerLocalAttachable
{
    public System.Action onUpdateLocal
    {
        get; set;
    }
}
