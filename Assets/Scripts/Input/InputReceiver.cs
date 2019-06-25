using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public abstract void ReceiveHorizontalInput(float value);

    public abstract void ReceiveVerticalInput(float value);
}
