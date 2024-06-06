using _Project.Scripts.Weapon;
using UnityEngine;

public interface IBullet
{
    public void SetupBullet(float xSpread, GameObject instigator, float range, BulletType bulletType);
}
