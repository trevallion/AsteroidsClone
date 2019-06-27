using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    private static float DefaultBulletVelocity = 5;
    private static float BulletFirePositionOffset = 0.2f;
    private const float DelayBetweenShots = 0.25f;

    [SerializeField]
    private Transform _shipTransform;

    private float LastShotTaken { get; set; }

    public void LaunchBullet()
    {
        if (LastShotTaken + DelayBetweenShots > Time.time)
        {
            return;
        }

        LastShotTaken = Time.time;
        Vector2 shipHeading = _shipTransform.up;
        Vector2 bulletPosition = (Vector2)_shipTransform.position + shipHeading * BulletFirePositionOffset;
        Bullet bullet = ObjectFactory.GetBullet();
        bullet.MoveTo(bulletPosition);
        bullet.Fire(shipHeading * DefaultBulletVelocity);
    }
}
