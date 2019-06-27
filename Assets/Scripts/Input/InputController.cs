using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private const float InputValue = 1.0f;
    private const KeyCode LeftInputKey = KeyCode.A;
    private const KeyCode RightInputKey = KeyCode.D;
    private const KeyCode ForwardInputKey = KeyCode.W;
    private const KeyCode ReverseInputKey = KeyCode.S;
    private const KeyCode ActionInputKey = KeyCode.Space;

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

    private void CheckActionInput()
    {
        if (Input.GetKey(ActionInputKey))
        {
            _inputReceiver.ReceiveActionInput();
        }
    }
}
