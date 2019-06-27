using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPoolableObject
{
    private enum AsteroidSizeType
    {
        Small,
        Medium,
        Large
    }

    private const int BulletLayer = 8;
    private const float NewAsteroidMinPhysicsMultiplier = 0.8f;
    private const float NewAsteroidMaxPhysicsMultiplier = 1.5f;

    [SerializeField]
    private Rigidbody2D _asteroidRigidbody;

    [SerializeField]
    private ParticleSystem _destroyParticles;

    [SerializeField]
    private Renderer _asteroidRenderer;

    [SerializeField]
    private Collider2D _collider2D;

    private AsteroidSizeType AsteroidSize { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == BulletLayer)
        {
            Explode();
        }
    }

    public void Activate()
    {
        _asteroidRenderer.enabled = true;
        _collider2D.enabled = true;
    }

    public void Deactivate()
    {
        _asteroidRigidbody.velocity = Vector2.zero;
        _asteroidRigidbody.angularVelocity = 0;
        _asteroidRenderer.enabled = false;
        _collider2D.enabled = false;
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

    private void Explode()
    {
        _destroyParticles?.Play();
        Deactivate();
        if (AsteroidSize != AsteroidSizeType.Small)
        {
            SpawnNewAsteroids();
        }
    }

    private void ReturnToPool()
    {
        switch (AsteroidSize)
        {
            case AsteroidSizeType.Small:
                ObjectFactory.ReturnSmallAsteroid(this);
                break;
            case AsteroidSizeType.Medium:
                ObjectFactory.ReturnMediumAsteroid(this);
                break;
            case AsteroidSizeType.Large:
                ObjectFactory.ReturnLargeAsteroid(this);
                break;
        }
    }

    private void SpawnNewAsteroids()
    {
        Vector2 heading = _asteroidRigidbody.velocity.normalized;
        // Get new headings perpendicular to original heading.
        Vector2 firstChildHeading = Vector3.Cross(heading, -heading);
        Vector2 secondChildHeading = -firstChildHeading;
        firstChildHeading *= GetRandomPhysicsMultiplier();
        secondChildHeading *= GetRandomPhysicsMultiplier();
        float firstChildAngularVelocity = _asteroidRigidbody.angularVelocity * GetRandomPhysicsMultiplier();
        float secondChildAngularVelocity = -_asteroidRigidbody.angularVelocity * GetRandomPhysicsMultiplier();


    }

    private void SpawnNewAsteroid(Vector2 heading, float angularVelocity)
    {
        Asteroid newAsteroid;
        switch (AsteroidSize)
        {
            case AsteroidSizeType.Medium:
                newAsteroid = ObjectFactory.GetSmallAsteroid();
                break;
            case AsteroidSizeType.Large:
                newAsteroid = ObjectFactory.GetMediumAsteroid();
                break;
            default:
                throw new System.InvalidOperationException("Attempted to spawn asteroid smaller than small");
        }
        newAsteroid.FlyInDirection(heading);
        newAsteroid.SetAngularVelocity(angularVelocity);
    }

    private float GetRandomPhysicsMultiplier()
    {
        return Random.Range(NewAsteroidMinPhysicsMultiplier, NewAsteroidMaxPhysicsMultiplier);
    }
}
