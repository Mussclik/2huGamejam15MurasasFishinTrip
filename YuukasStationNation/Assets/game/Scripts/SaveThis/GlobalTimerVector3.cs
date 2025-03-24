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
        Start();
    }
    public GlobalTimerVector3(float newDuration = 0) : base(newDuration)
    {
        Start();
    }

    public void ChangeVectors(Vector3 newBeginningVector, Vector3 newEndingVector)
    {
        beginningVector = newBeginningVector;
        endingVector = newEndingVector;
    }
    public override void Start()
    {
        callOnStart += () => vectorToChange(beginningVector);
        callOnUpdate += () => vectorToChange(Vector3.Lerp(beginningVector, endingVector, animationCurve.Evaluate(PercentFinished)));
        callOnFinish += () => vectorToChange(endingVector);
    }
}
