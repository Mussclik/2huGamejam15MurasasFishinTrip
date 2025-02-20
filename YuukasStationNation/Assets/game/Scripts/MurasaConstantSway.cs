using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurasaConstantSway : MonoBehaviour
{
    [SerializeField] private float angles = 20f; // Max rotation angle
    [SerializeField] private float swaySpeed = 1f; // Speed of sway
    [SerializeField] private AnimationCurve curve; // Custom sway curve

    private float timeElapsed = 0f;

    void Update()
    {
        timeElapsed += Time.deltaTime * swaySpeed;

        // Evaluate curve (normalized time) and map to rotation range
        float swayFactor = curve.Evaluate(timeElapsed % 1f); // Ensure time loops between 0 and 1
        float targetYRotation = Mathf.Lerp(-angles, angles, swayFactor);

        // Apply rotation only to Y-axis while keeping other axes unchanged
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,  targetYRotation);
    }
}
