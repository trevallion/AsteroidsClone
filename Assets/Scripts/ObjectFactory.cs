using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    private const int DefaultNumberOfSmallAsteroids = 40;
    private const int DefaultNumberOfMediumAsteroids = 20;
    private const int DefaultNumberOfLargeAsteroids = 10;
    private const int DefaultNumberOfBullets = 15;

    private static readonly Vector3 ObjectPoolDefaultPosition = new Vector3(100, 100, 0);

    [SerializeField]
    private GameObject _smallAsteroidPrefab;

    [SerializeField]
    private GameObject _mediumAsteroidPrefab;

    [SerializeField]
    private GameObject _largeAsteroidPrefab;

    [SerializeField]
    private GameObject _bulletPrefab;

    private static ObjectFactory Instance { get; set; }

    private ObjectPool<Asteroid> SmallAsteroidPool { get; set; }

    private ObjectPool<Asteroid> MediumAsteroidPool { get; set; }

    private ObjectPool<Asteroid> LargeAsteroidPool { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SmallAsteroidPool = new ObjectPool<Asteroid>(_smallAsteroidPrefab, ObjectPoolDefaultPosition, DefaultNumberOfSmallAsteroids);
        MediumAsteroidPool = new ObjectPool<Asteroid>(_mediumAsteroidPrefab, ObjectPoolDefaultPosition, DefaultNumberOfMediumAsteroids);
        LargeAsteroidPool = new ObjectPool<Asteroid>(_largeAsteroidPrefab, ObjectPoolDefaultPosition, DefaultNumberOfLargeAsteroids);
    }

    public static void ReturnSmallAsteroid(Asteroid asteroid)
    {
        Instance.SmallAsteroidPool.Return(asteroid);
    }

    public static void ReturnMediumAsteroid(Asteroid asteroid)
    {
        Instance.MediumAsteroidPool.Return(asteroid);
    }

    public static void ReturnLargeAsteroid(Asteroid asteroid)
    {
        Instance.LargeAsteroidPool.Return(asteroid);
    }

    public static Asteroid GetSmallAsteroid()
    {
        return Instance.SmallAsteroidPool.Retrieve();
    }

    public static Asteroid GetMediumAsteroid()
    {
        return Instance.MediumAsteroidPool.Retrieve();
    }

    public static Asteroid GetLargeAsteroid()
    {
        return Instance.LargeAsteroidPool.Retrieve();
    }
}
