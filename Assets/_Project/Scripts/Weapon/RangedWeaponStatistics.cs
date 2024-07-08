
using System;

namespace _Project.Scripts.Weapon
{
    [Serializable]
    public struct RangedWeaponStatistics
    {
        public RangedWeaponStatistics(float waitingTimeForNextShoot, BulletType bulletType, 
            SpreadType spreadType, float spread, float spreadStep, float spreadResetThreshold, 
            int projectilesPerShot, int magazineSize, float range)
        {
            WaitingTimeForNextShoot = waitingTimeForNextShoot;
            BulletType = bulletType;
            SpreadType = spreadType;
            Spread = spread;
            SpreadStep = spreadStep;
            SpreadResetThreshold = spreadResetThreshold;
            ProjectilesPerShot = projectilesPerShot;
            MagazineSize = magazineSize;
            Range = range;
        }

        public float WaitingTimeForNextShoot;
        public BulletType BulletType;
        public SpreadType SpreadType;
        public float Spread;
        public float SpreadStep;
        public float SpreadResetThreshold;
        public int ProjectilesPerShot;
        public int MagazineSize;
        public float Range;
    }
}
