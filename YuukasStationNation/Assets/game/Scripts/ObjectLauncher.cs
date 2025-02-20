using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ObjectLauncher
[Serializable]
public class ObjectLauncher
{
    public float launchSpeed = 4.4f; // Speed at which the object will be launched
    public float rotationSpeed = 335f; // Base rotation speed of the object

    // Method to launch the object
    public GameObject LaunchObject(GameObject objectToYeet)
    {
        GameObject instance = MonoBehaviour.Instantiate(objectToYeet, objectToYeet.transform.position, objectToYeet.transform.rotation);

        // Generate a random launch angle in 2D space (X-Y only)
        float angle = UnityEngine.Random.Range(40f, 140f);
        float angleRad = angle * Mathf.Deg2Rad;

        // Compute force in X and Y directions only (Z is always 0)
        Vector3 force = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * launchSpeed;

        Rigidbody rb = instance.GetComponent<Rigidbody>();
        if (rb == null)
            rb = instance.AddComponent<Rigidbody>();

        rb.useGravity = true; // Enable gravity
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        // Freezes position in Z direction and prevents rotation on X & Y axes

        rb.AddForce(force, ForceMode.Impulse);

        // Apply torque only around the Z-axis
        float rotationSpeedModifier = UnityEngine.Random.Range(0.7f, 1.3f);
        if (UnityEngine.Random.Range(0, 2) == 1)
            rotationSpeedModifier *= -1;

        rb.AddTorque(Vector3.forward * rotationSpeed * rotationSpeedModifier, ForceMode.Impulse);

        MonoBehaviour.Destroy(instance, 5f);
        return instance;
    }
}
#endregion
