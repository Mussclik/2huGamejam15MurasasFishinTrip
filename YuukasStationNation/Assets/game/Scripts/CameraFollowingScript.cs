using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingScript : MonoBehaviour
{
    [SerializeField] public float speed = 0.2f;
    [SerializeField] private Transform positionToFollow;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, positionToFollow.position, speed * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(positionToFollow.position, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.35f);

    }
}
