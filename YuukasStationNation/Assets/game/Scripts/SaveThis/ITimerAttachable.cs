using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimerStaticAttachable
{
    public static System.Action onUpdate;
}
public interface ITimerLocalAttachable
{
    public System.Action onUpdate { get; set; }
}
