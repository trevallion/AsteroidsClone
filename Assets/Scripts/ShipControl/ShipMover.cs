using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour
{
    private static float TurnSpeed = 1.5f;
    private static float AccelerationRate = 0.9f;
    private static float MaxVelocity = 5f;

    [SerializeField]
    private Rigidbody2D _shipRigidbody;
    [SerializeField]
    private Transform _shipTransform;

    public void RotateShip(float rotationAmount)
    {
        _shipRigidbody.rotation -= rotationAmount * TurnSpeed;
    }

    public void ChangeShipVelocity(float velocityAmount)
    {
        // TODO: Cache value and apply in FixedUpdate
        float currentVelocityMagnitude = _shipRigidbody.velocity.magnitude;
        Vector2 forward = _shipTransform.up;
        _shipRigidbody.AddForce(forward * velocityAmount * AccelerationRate);
    }
}
