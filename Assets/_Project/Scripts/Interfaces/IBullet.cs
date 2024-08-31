using _Project.Scripts.Weapons.Upgrades.Bullet;
using UnityEngine;

public interface IBullet
{
    public void SetupBullet(float xSpread, GameObject instigator, float range, BulletType bulletType);
}
