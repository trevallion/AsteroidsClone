using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const int BulletLayer = 8;

    [SerializeField]
    private GameObject _spawnOnDestroy;

    [SerializeField]
    private GameObject _asteroidTransform;

    [SerializeField]
    private ParticleSystem _destroyParticles;

    [SerializeField]
    private Renderer _asteroidRenderer;

    [SerializeField]
    private Collider2D _collider2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == BulletLayer)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _asteroidRenderer.enabled = false;
        _collider2D.enabled = false;
        _destroyParticles.Play();

    }

    private void SpawnNewAsteroids()
    {

    }
}
