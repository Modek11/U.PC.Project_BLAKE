using System;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [Serializable]
    public struct RangedWeaponStatistics
    {
        public RangedWeaponStatistics(float waitingTimeForNextShoot, BulletType bulletType, 
            SpreadType spreadType, float spread, float spreadStep, float spreadThreshold, float spreadResetThreshold, 
            int projectilesPerShot, int magazineSize, float range)
        {
            WaitingTimeForNextShoot = waitingTimeForNextShoot;
            BulletType = bulletType;
            SpreadType = spreadType;
            Spread = spread;
            SpreadStep = spreadStep;
            SpreadThreshold = spreadThreshold;
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
        public float SpreadThreshold;
        public float SpreadResetThreshold;
        public int ProjectilesPerShot;
        public int MagazineSize;
        public float Range;
        
        public static RangedWeaponStatistics operator +(RangedWeaponStatistics a, RangedWeaponStatistics b)
        {
            if ((a.BulletType != BulletType.Undefined && b.BulletType != BulletType.Undefined) || 
                (a.SpreadType != SpreadType.Undefined && b.SpreadType != SpreadType.Undefined))
            {
                Debug.LogError(
                    "You are trying to combine two different BulletTypes or SpreadTypes. Returning Types from first variable!");
            }
            
            return new RangedWeaponStatistics(
                a.WaitingTimeForNextShoot + b.WaitingTimeForNextShoot,
                a.BulletType, // Assuming bulletType should remain unchanged
                a.SpreadType, // Assuming spreadType should remain unchanged
                a.Spread + b.Spread,
                a.SpreadStep + b.SpreadStep,
                a.SpreadThreshold + b.SpreadThreshold,
                a.SpreadResetThreshold + b.SpreadResetThreshold,
                a.ProjectilesPerShot + b.ProjectilesPerShot,
                a.MagazineSize + b.MagazineSize,
                a.Range + b.Range
            );
        }
    }
}
