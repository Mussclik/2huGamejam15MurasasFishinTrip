using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float drag = 1f;

    [SerializeField] private Vector2 currentVelocity = Vector2.zero;
    [SerializeField] private Vector2 desiredDirection = Vector2.zero;

    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private float boatTiltX = 20f;
    [SerializeField] private float boatTiltY = 30f;
    [SerializeField] private Transform boatVisuals;
    [SerializeField] private float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Update()
    {
        if (GameManager.gamestate == Gamestate.MovingOnMap)
        {
            MovementCheck();
        }
    }

    private void FixedUpdate()
    {
        // Apply acceleration independently from max speed
        if (desiredDirection != Vector2.zero)
        {
            Vector2 accelerationVector = desiredDirection * acceleration * Time.fixedDeltaTime;
            currentVelocity += accelerationVector;
        }
        else
        {
            // Apply drag when no input
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, drag * Time.fixedDeltaTime);
        }

        // Clamp velocity to maxSpeed
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }

        rigidBody.velocity = new Vector3(currentVelocity.x, 0, currentVelocity.y);
        RotateBoatVisuals();
    }

    private void MovementCheck()
    {
        desiredDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) desiredDirection.y += 1;
        if (Input.GetKey(KeyCode.S)) desiredDirection.y -= 1;
        if (Input.GetKey(KeyCode.D)) desiredDirection.x += 1;
        if (Input.GetKey(KeyCode.A)) desiredDirection.x -= 1;
        desiredDirection.Normalize();
    }

    void RotateBoatVisuals()
    {
        if (!boatVisuals) return; // Prevent errors if boatVisuals is not assigned

        float targetXRotation = 0;
        float targetYRotation = 0;

        // Handle X tilt (left/right movement)
        if (currentVelocity.x > 0)
        {
            targetXRotation = -boatTiltX;
            targetYRotation = 180; // Facing right
        }
        else if (currentVelocity.x < 0)
        {
            targetXRotation = boatTiltX;
            targetYRotation = 0; // Facing left
        }

        // Handle Y tilt (forward/backward movement), relative to boat direction
        if (currentVelocity.y > 6)
        {
            targetYRotation += (currentVelocity.x > 0) ? -boatTiltY : boatTiltY; // Adjust depending on facing direction
        }
        else if (currentVelocity.y < -6)
        {
            targetYRotation += (currentVelocity.x > 0) ? boatTiltY : -boatTiltY; // Adjust depending on facing direction
        }

        // Smoothly interpolate rotation
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);
        boatVisuals.rotation = Quaternion.Lerp(boatVisuals.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void TryMoveDirection(Vector2 movingDirection)
    {
        Physics.Raycast(transform.position, new Vector3(movingDirection.x, 0, movingDirection.y), 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), new Vector3(desiredDirection.x, 0, desiredDirection.y) * 3);
    }
}
