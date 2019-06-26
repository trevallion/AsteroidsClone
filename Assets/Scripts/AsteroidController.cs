using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private const int DefaultAsteroidCount = 8;
    private const float AngularVelocityCap = 0.8f;

    [SerializeField]
    private float _xBoundary;

    [SerializeField]
    private float _yBoundary;

    private int AsteroidCount { get; set; }

    private void Awake()
    {
        AsteroidCount = DefaultAsteroidCount;
    }

    public void CreateNewAsteroids()
    {
        for (int i = 0; i < AsteroidCount; i++)
        {
            CreateNewAsteroid();
        }

    }

    public void ResetAsteroidCount()
    {
        AsteroidCount = DefaultAsteroidCount;
    }

    public void IncrementAsteroidCount()
    {
        AsteroidCount++;
    }

    private void CreateNewAsteroid()
    {
        Asteroid asteroid = ObjectFactory.GetLargeAsteroid();
        Vector2 direction = Random.onUnitSphere;
        float angularVelocity = Random.Range(-AngularVelocityCap, AngularVelocityCap);
        asteroid.FlyInDirection(direction);
        asteroid.SetAngularVelocity(angularVelocity);
    }
}
