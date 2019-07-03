using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    private const int DefaultNumberOfBullets = 15;

    private static readonly Vector3 ObjectPoolDefaultPosition = new Vector3(100, 100, 0);

    [SerializeField]
    private GameObject _bulletPrefab;

    private static ObjectFactory Instance { get; set; }

    private ObjectPool<Bullet> BulletPool { get; set; }

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
        BulletPool = new ObjectPool<Bullet>(_bulletPrefab, ObjectPoolDefaultPosition, DefaultNumberOfBullets);
    }

    public static void ReturnBullet(Bullet bullet)
    {
        bullet.Deactivate();
        Instance.BulletPool.Return(bullet);
    }

    public static Bullet GetBullet()
    {
        Bullet bullet = Instance.BulletPool.Retrieve();
        bullet.Activate();
        return bullet;
    }
}
