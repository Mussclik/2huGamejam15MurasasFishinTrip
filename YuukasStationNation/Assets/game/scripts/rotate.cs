using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float velocity;
    public int direction = 1;
    void Update()
    {
        transform.Rotate(0, 0, velocity * Time.deltaTime);
        velocity += direction;
        if (velocity >= 720)
        {
            direction = -1;
        }
        else if (velocity <= -720)
        {
            direction = 1;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
