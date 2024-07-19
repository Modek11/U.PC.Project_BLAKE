using _Project.Scripts.Weapon.Upgrades.Bullet;
using UnityEngine;

public interface IBullet
{
    public void SetupBullet(float xSpread, GameObject instigator, float range, BulletType bulletType);
}
