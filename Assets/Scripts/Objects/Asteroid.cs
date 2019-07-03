using System;
using UnityEngine;

public class AsteroidStateChangedEventArgs : EventArgs
{
    public Asteroid Source { get; set; }
    public bool IsAlive { get; set; }
    public bool DestroyedByPlayer { get; set; }

    public AsteroidStateChangedEventArgs() { }

    public AsteroidStateChangedEventArgs(Asteroid source, bool isAlive, bool destroyedByPlayer)
    {
        Source = source;
        IsAlive = isAlive;
        DestroyedByPlayer = destroyedByPlayer;
    }
}

public enum AsteroidSizeType
{
    Small,
    Medium,
    Large
}

public class Asteroid : MonoBehaviour, IPoolableObject, IObservable<AsteroidStateChangedEventArgs>
{
    private const int BulletLayer = 8;

    public event Action<AsteroidStateChangedEventArgs> StateChanged;

    [SerializeField]
    private Rigidbody2D _asteroidRigidbody;

    [SerializeField]
    private Renderer _asteroidRenderer;

    [SerializeField]
    private Collider2D _collider2D;

    [SerializeField]
    private AsteroidSizeType _asteroidSize;

    public AsteroidSizeType AsteroidSize
    {
        get
        {
            return _asteroidSize;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return _asteroidRigidbody.velocity;
        }

        set
        {
            _asteroidRigidbody.velocity = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _asteroidRigidbody.position;
        }
    }

    public float AngularVelocity
    {
        get
        {
            return _asteroidRigidbody.angularVelocity;
        }

        set
        {
            _asteroidRigidbody.angularVelocity = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == BulletLayer)
        {
            Explode();
        }
    }

    public void Activate()
    {
        SetActive(true);
        AsteroidStateChangedEventArgs eventArgs = new AsteroidStateChangedEventArgs(this, true, false);
        StateChanged?.Invoke(eventArgs);
    }

    public void Deactivate()
    {
        Deactivate(false);
    }

    public void Deactivate(bool destroyedByPlayer)
    {
        SetActive(false);
    }

    public void MoveTo(Vector3 position)
    {
        _asteroidRigidbody.position = position;
    }

    public void FlyInDirection(Vector2 direction)
    {
        _asteroidRigidbody.velocity = direction;
    }

    public void SetAngularVelocity(float newVelocity)
    {
        _asteroidRigidbody.angularVelocity = newVelocity;
    }

    public void ReturnToPool()
    {
        AsteroidStateChangedEventArgs eventArgs = new AsteroidStateChangedEventArgs(this, false, false);
        StateChanged?.Invoke(eventArgs);
    }

    private void Explode()
    {
        AsteroidStateChangedEventArgs eventArgs = new AsteroidStateChangedEventArgs(this, false, true);
        StateChanged?.Invoke(eventArgs);
        Deactivate(true);
    }

    private void SetActive(bool active)
    {
        _asteroidRenderer.enabled = active;
        _collider2D.enabled = active;
        if (!active)
        {
            _asteroidRigidbody.velocity = Vector2.zero;
            _asteroidRigidbody.angularVelocity = 0;
        }

    }
}
