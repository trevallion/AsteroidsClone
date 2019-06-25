using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : InputReceiver
{
    private static float TurnSpeed = 0.75f;
    private static float AccelerationRate = 0.5f;
    private static float MaxVelocity = 5f;

    [SerializeField]
    private Rigidbody2D _shipRigidbody;
    [SerializeField]
    private Transform _shipTransform;

    public override void ReceiveHorizontalInput(float value)
    {
        RotateShip(TurnSpeed * value);
    }

    public override void ReceiveVerticalInput(float value)
    {
        ChangeShipVelocity(value * AccelerationRate);
    }

    private void RotateShip(float rotationAmount)
    {
        _shipRigidbody.rotation -= rotationAmount;
    }

    private void ChangeShipVelocity(float velocityAmount)
    {
        float currentVelocityMagnitude = _shipRigidbody.velocity.magnitude;
        Vector2 forward = _shipTransform.up;

        if (currentVelocityMagnitude < MaxVelocity)
        {
            _shipRigidbody.AddForce(forward * velocityAmount);
        }
        else
        {
            _shipRigidbody.velocity = Vector3.RotateTowards(_shipRigidbody.velocity, forward * MaxVelocity, velocityAmount, AccelerationRate);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
