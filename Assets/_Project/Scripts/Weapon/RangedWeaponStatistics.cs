using System;
using System.Collections.Generic;
using _Project.Scripts.Weapon.Upgrades.Bullet;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [Serializable]
    public struct RangedWeaponStatistics : IWeaponStatistics
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
        
        public Dictionary<string, float> GetNonZeroFields()
        {
            var result = new Dictionary<string, float>();

            if (WaitingTimeForNextShoot != 0)
                result.Add(nameof(WaitingTimeForNextShoot), WaitingTimeForNextShoot);

            if (BulletType != BulletType.Undefined)
                result.Add(nameof(BulletType), (float)BulletType);

            if (SpreadType != SpreadType.Undefined)
                result.Add(nameof(SpreadType), (float)SpreadType);

            if (Spread != 0)
                result.Add(nameof(Spread), Spread);

            if (SpreadStep != 0)
                result.Add(nameof(SpreadStep), SpreadStep);

            if (SpreadThreshold != 0)
                result.Add(nameof(SpreadThreshold), SpreadThreshold);

            if (SpreadResetThreshold != 0)
                result.Add(nameof(SpreadResetThreshold), SpreadResetThreshold);

            if (ProjectilesPerShot != 0)
                result.Add(nameof(ProjectilesPerShot), ProjectilesPerShot);

            if (MagazineSize != 0)
                result.Add(nameof(MagazineSize), MagazineSize);

            if (Range != 0)
                result.Add(nameof(Range), Range);

            return result;
        }
        
        public bool IsNullOrEmpty()
        {
            return WaitingTimeForNextShoot != 0 ||
                   BulletType != BulletType.Undefined ||
                   SpreadType != SpreadType.Undefined ||
                   Spread != 0 ||
                   SpreadStep != 0 ||
                   SpreadThreshold != 0 ||
                   SpreadResetThreshold != 0 ||
                   ProjectilesPerShot != 0 ||
                   MagazineSize != 0 ||
                   Range != 0;
        }
    }
}
