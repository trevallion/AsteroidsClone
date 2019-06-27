using UnityEngine;

public class Bullet : MonoBehaviour, IPoolableObject
{
    private const float BulletDuration = 1.5f;
    private const string ReturnToPoolMethodName = "ReturnToPool";

    [SerializeField]
    private Rigidbody2D _bulletRigidbody;

    [SerializeField]
    private Collider2D _bulletCollider;

    private bool IsActive { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    public void Activate()
    {
        IsActive = true;
        _bulletCollider.enabled = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        _bulletRigidbody.velocity = Vector2.zero;
        _bulletRigidbody.angularVelocity = 0;
        _bulletCollider.enabled = false;
    }

    public void MoveTo(Vector3 position)
    {
        _bulletRigidbody.position = position;
    }

    public void Fire(Vector2 velocity)
    {
        _bulletRigidbody.velocity = velocity;
        Invoke(ReturnToPoolMethodName, BulletDuration);
    }

    private void ReturnToPool()
    {
        if (IsActive)
        {
            ObjectFactory.ReturnBullet(this);
        }
    }
}
