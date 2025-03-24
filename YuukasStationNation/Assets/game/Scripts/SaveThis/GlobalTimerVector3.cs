using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimerVector3 : GlobalTimer
{
    [SerializeField] private Vector3 beginningVector = Vector3.zero;
    [SerializeField] private Vector3 endingVector = Vector3.one;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
    public System.Action<Vector3> vectorToChange;

    public GlobalTimerVector3(ITimerLocalAttachable scriptToAttachTimer, float newDuration = 0) : base(scriptToAttachTimer, newDuration)
    {
        VirtualStart();
    }
    public GlobalTimerVector3(float newDuration = 0) : base(newDuration)
    {
        VirtualStart();
    }

    public void ChangeVectors(Vector3 newBeginningVector, Vector3 newEndingVector)
    {
        beginningVector = newBeginningVector;
        endingVector = newEndingVector;
    }
    protected override void VirtualStart()
    {
        callOnStart += () => vectorToChange(beginningVector);
        callOnUpdate += () => vectorToChange(Vector3.Lerp(beginningVector, endingVector, animationCurve.Evaluate(PercentFinished)));
        callOnFinish += () => vectorToChange(endingVector);
    }
}

