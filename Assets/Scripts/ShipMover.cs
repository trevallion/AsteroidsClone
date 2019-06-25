using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : InputReceiver
{
    private static float TurnSpeed = 1.5f;
    private static float AccelerationRate = 0.9f;
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
        // TODO: Cache value and apply in FixedUpdate
        float currentVelocityMagnitude = _shipRigidbody.velocity.magnitude;
        Vector2 forward = _shipTransform.up;
        _shipRigidbody.AddForce(forward * velocityAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
