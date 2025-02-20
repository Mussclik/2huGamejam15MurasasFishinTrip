using UnityEngine;

public class SwayMotion : MonoBehaviour
{
    [Header("X-Axis Sway")]
    [SerializeField] private float amplitudeX = 0.5f; // How far it moves on X
    [SerializeField] private float speedX = 1f; // How fast it moves on X

    [Header("Y-Axis Sway")]
    [SerializeField] private float amplitudeY = 0.5f; // How far it moves on Y
    [SerializeField] private float speedY = 1f; // How fast it moves on Y

    [SerializeField] private Transform anchorPosition; // Reference for base position

    void Update()
    {
        float swayOffsetX = Mathf.Sin(Time.time * speedX) * amplitudeX;
        float swayOffsetY = Mathf.Sin(Time.time * speedY) * amplitudeY;

        transform.position = anchorPosition.position + new Vector3(swayOffsetX, swayOffsetY, 0);
    }
}
