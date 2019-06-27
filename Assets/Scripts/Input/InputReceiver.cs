using UnityEngine;
using UnityEngine.Events;

public class InputReceiver : MonoBehaviour
{
    [SerializeField]
    private UnityFloatEvent _horizontalInputEvent;

    [SerializeField]
    private UnityFloatEvent _verticalInputEvent;

    [SerializeField]
    private UnityEvent _actionInputEvent;

    public void ReceiveHorizontalInput(float value)
    {
        _horizontalInputEvent?.Invoke(value);
    }

    public void ReceiveVerticalInput(float value)
    {
        _verticalInputEvent?.Invoke(value);
    }

    public void ReceiveActionInput()
    {
        _actionInputEvent?.Invoke();
    }
}
