using UnityEngine;

public class ObjectScreenWrapper : MonoBehaviour
{
    private const float XBounds = 9.5f;
    private const float YBounds = 5.5f;

    [SerializeField]
    private Renderer _objectRenderer;

    [SerializeField]
    private Transform _objectTransform;

    private bool IsOffscreen
    {
        get
        {
            return _objectTransform.position.x < -XBounds || _objectTransform.position.x > XBounds ||
                _objectTransform.position.y < -YBounds || _objectTransform.position.y > YBounds;
        }
    }

    private void Update()
    {
        if (IsOffscreen)
        {
            WrapAround();
        }
    }

    private void WrapAround()
    {
        Vector3 objectPosition = _objectTransform.position;

        if(objectPosition.x < -XBounds)
        {
            objectPosition.x = XBounds;
        }
        else if(objectPosition.x > XBounds)
        {
            objectPosition.x = -XBounds;
        }

        if (objectPosition.y < -YBounds)
        {
            objectPosition.y = YBounds;
        }
        else if (objectPosition.y > YBounds)
        {
            objectPosition.y = -YBounds;
        }

        _objectTransform.position = objectPosition;
    }
}
