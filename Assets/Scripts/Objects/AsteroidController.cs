using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IObserver<AsteroidStateChangedEventArgs>
{
    // This class is needed so we can see the field in the inspector.
    [System.Serializable]
    public class AsteroidPool : ObjectPool<Asteroid> { }

    private const int DefaultAsteroidCount = 8;
    private const float AngularVelocityCap = 5f;
    private const int DefaultNumberOfSmallAsteroids = 40;
    private const int DefaultNumberOfMediumAsteroids = 20;
    private const int DefaultNumberOfLargeAsteroids = 10;
    private const float MinPhysicsMultiplier = 0.5f;
    private const float MaxPhysicsMultiplier = 1.25f;

    private static readonly Vector3 ObjectPoolPosition = new Vector3(100, 100, 0);

    [SerializeField]
    private float _xBoundary;

    [SerializeField]
    private float _yBoundary;

    [SerializeField]
    private AsteroidPool _smallAsteroidPool;

    [SerializeField]
    private AsteroidPool _mediumAsteroidPool;

    [SerializeField]
    private AsteroidPool _largeAsteroidPool;

    private int AsteroidCount { get; set; }

    public List<Asteroid> ActiveAsteroids { get; set; }

    private void Awake()
    {
        AsteroidCount = DefaultAsteroidCount;
        ActiveAsteroids = new List<Asteroid>();
        _smallAsteroidPool.Initialize(ObjectPoolPosition, DefaultNumberOfSmallAsteroids);
        SubscribeToEvents(_smallAsteroidPool);
        _mediumAsteroidPool.Initialize(ObjectPoolPosition, DefaultNumberOfMediumAsteroids);
        SubscribeToEvents(_mediumAsteroidPool);
        _largeAsteroidPool.Initialize(ObjectPoolPosition, DefaultNumberOfLargeAsteroids);
        SubscribeToEvents(_largeAsteroidPool);
        SpawnAllNewAsteroids();
    }

    public void ResetAsteroidCount()
    {
        AsteroidCount = DefaultAsteroidCount;
    }

    public void IncrementAsteroidCount()
    {
        AsteroidCount++;
    }

    public void OnStateChanged(AsteroidStateChangedEventArgs eventArgs)
    {
        if (eventArgs.IsAlive)
        {
            ActiveAsteroids.Add(eventArgs.Source);
        }
        else
        {
            ActiveAsteroids.Remove(eventArgs.Source);
            if (eventArgs.DestroyedByPlayer)
            {
                if (eventArgs.Source.AsteroidSize != AsteroidSizeType.Small)
                {
                    SpawnChildAsteroids(eventArgs.Source);
                }

                if (ActiveAsteroids.Count == 0)
                {
                    AsteroidCount++;
                    SpawnAllNewAsteroids();
                }
            }
            ReturnAsteroidToPool(eventArgs.Source);
        }
    }

    private void SpawnAllNewAsteroids()
    {
        Vector3 position;
        Vector2 direction;
        float angularVelocity;
        for (int i = 0; i < AsteroidCount; i++)
        {
            position = GetRandomSpawnPosition();
            direction = Random.onUnitSphere;
            angularVelocity = Random.Range(-AngularVelocityCap, AngularVelocityCap);
            SpawnNewAsteroid(AsteroidSizeType.Large, position, direction, angularVelocity);
        }
    }

    private void SpawnChildAsteroids(Asteroid asteroid)
    {
        AsteroidSizeType childAsteroidSize = GetNextSmallestAsteroidSize(asteroid.AsteroidSize);
        Vector2 parentVelocity = asteroid.Velocity;
        Vector2 firstChildVelocity = new Vector2(parentVelocity.y, -parentVelocity.x);
        Vector2 secondChildVelocity = -firstChildVelocity;
        firstChildVelocity *= GetRandomPhysicsMultiplier();
        secondChildVelocity *= GetRandomPhysicsMultiplier();
        float firstChildAngularVelocity = asteroid.AngularVelocity * GetRandomPhysicsMultiplier();
        float secondChildAngularVelocity = -asteroid.AngularVelocity * GetRandomPhysicsMultiplier();
        SpawnNewAsteroid(childAsteroidSize, asteroid.Position, firstChildVelocity, firstChildAngularVelocity);
        SpawnNewAsteroid(childAsteroidSize, asteroid.Position, secondChildVelocity, secondChildAngularVelocity);
    }

    private void SpawnNewAsteroid(AsteroidSizeType asteroidSize, Vector3 position, Vector2 direction, float angularVelocity)
    {
        Asteroid asteroid = GetAsteroidFromPool(asteroidSize);
        asteroid.Activate();
        asteroid.MoveTo(position);
        asteroid.FlyInDirection(direction);
        asteroid.SetAngularVelocity(angularVelocity);
    }

    private Asteroid GetAsteroidFromPool(AsteroidSizeType asteroidSize)
    {
        switch (asteroidSize)
        {
            case AsteroidSizeType.Small:
                return _smallAsteroidPool.Retrieve();
            case AsteroidSizeType.Medium:
                return _mediumAsteroidPool.Retrieve();
            case AsteroidSizeType.Large:
                return _largeAsteroidPool.Retrieve();
            default:
                throw new System.InvalidOperationException("Invalid asteroid type given.");
        }
    }

    private void ReturnAsteroidToPool(Asteroid asteroid)
    {
        switch (asteroid.AsteroidSize)
        {
            case AsteroidSizeType.Small:
                _smallAsteroidPool.Return(asteroid);
                break;
            case AsteroidSizeType.Medium:
                _mediumAsteroidPool.Return(asteroid);
                break;
            case AsteroidSizeType.Large:
                _largeAsteroidPool.Return(asteroid);
                break;
        }
    }

    private AsteroidSizeType GetNextSmallestAsteroidSize(AsteroidSizeType asteroidSize)
    {
        switch (asteroidSize)
        {
            case AsteroidSizeType.Medium:
                return AsteroidSizeType.Small;
            case AsteroidSizeType.Large:
                return AsteroidSizeType.Medium;
            default:
                throw new System.InvalidOperationException("Something went wrong while creating new asteroids.");
        }
    }

    private float GetRandomPhysicsMultiplier()
    {
        return Random.Range(MinPhysicsMultiplier, MaxPhysicsMultiplier);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        return new Vector2(Random.Range(-_xBoundary, _xBoundary), Random.Range(-_yBoundary, _yBoundary));
    }

    private void SubscribeToEvents(AsteroidPool asteroidPool)
    {
        foreach (Asteroid asteroid in asteroidPool.AllObjects)
        {
            asteroid.StateChanged += OnStateChanged;
        }
    }
}
