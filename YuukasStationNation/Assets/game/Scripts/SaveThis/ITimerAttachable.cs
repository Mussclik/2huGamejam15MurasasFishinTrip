using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimerStaticAttachable
{
    public static System.Action onUpdateStatic;
    public static MonoBehaviour refrenceScript;

    public void DeclareAction();
}
public interface ITimerLocalAttachable
{
    public System.Action onUpdateLocal
    {
        get; set;
    }
}
