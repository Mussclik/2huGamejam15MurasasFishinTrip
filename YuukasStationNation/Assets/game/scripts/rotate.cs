using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float velocity = 0f;
    public float speedMultiplier = 1f;
    public float maxVelocity = 720f;
    public int direction = 1;

    private void OnEnable()
    {
        ResetState();
    }
    void Update()
    {
        Debug.Log("santaqhabibib");
        transform.Rotate(0, 0, velocity * Time.deltaTime);
        velocity += direction * speedMultiplier;
        if (velocity >= maxVelocity * speedMultiplier * ((speedMultiplier > 1) ? 5 : 1)) // this is truly horrendous
        {
            direction = -1;
        }
        else if (velocity <= -maxVelocity * speedMultiplier)
        {
            direction = 1;
        }
    }
    public void ResetState()
    {
        velocity = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
