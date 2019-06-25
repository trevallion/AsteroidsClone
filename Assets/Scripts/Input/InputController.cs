using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private const float InputValue = 1.0f;
    private static KeyCode LeftInputKey = KeyCode.A;
    private static KeyCode RightInputKey = KeyCode.D;
    private static KeyCode ForwardInputKey = KeyCode.W;
    private static KeyCode ReverseInputKey = KeyCode.S;

    [SerializeField]
    private InputReceiver _inputReceiver;

    private void Update()
    {
        CheckVerticalInput();
        CheckHorizontalInput();
    }

    private void CheckVerticalInput()
    {
        if (Input.GetKey(ReverseInputKey))
        {
            _inputReceiver.ReceiveVerticalInput(-InputValue);
        }
            
        if(Input.GetKey(ForwardInputKey))
        {
            _inputReceiver.ReceiveVerticalInput(InputValue);
        }
    }

    private void CheckHorizontalInput()
    {
        if (Input.GetKey(LeftInputKey))
        {
            _inputReceiver.ReceiveHorizontalInput(-InputValue);
        }

        if (Input.GetKey(RightInputKey))
        {
            _inputReceiver.ReceiveHorizontalInput(InputValue);
        }
    }
}
