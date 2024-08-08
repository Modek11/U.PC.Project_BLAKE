using System;
using System.Collections.Generic;
using _Project.Scripts.Weapon.Upgrades.Bullet;
using UnityEngine;

namespace _Project.Scripts.Weapon.Statistics
{
    [Serializable]
    public struct RangedWeaponStatistics : IWeaponStatistics
    {
        public RangedWeaponStatistics(float waitingTimeForNextShoot, BulletType bulletType, 
            SpreadType spreadType, float spread, float spreadStep, float spreadThreshold, float spreadResetThreshold, 
            int projectilesPerShot, int magazineSize, float range, float loudnessRange, LayerMask enemyLayerMask)
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
            LoudnessRange = loudnessRange;
            EnemyLayerMask = enemyLayerMask;
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
        public float LoudnessRange;
        public LayerMask EnemyLayerMask;
        
        public static RangedWeaponStatistics operator +(RangedWeaponStatistics a, RangedWeaponStatistics b)
        {
            if ((a.BulletType != BulletType.Undefined && b.BulletType != BulletType.Undefined) || 
                (a.SpreadType != SpreadType.Undefined && b.SpreadType != SpreadType.Undefined) ||
                (a.EnemyLayerMask != 0 && b.EnemyLayerMask != 0))
            {
                Debug.LogError(
                    "You are trying to combine two different BulletTypes or SpreadTypes or LayerMask. Returning Types from first variable!");
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
                a.Range + b.Range,
                a.LoudnessRange + b.LoudnessRange,
                a.EnemyLayerMask // Assuming layerMask should remain unchanged
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
            
            if (LoudnessRange != 0)
                result.Add(nameof(LoudnessRange), LoudnessRange);
            
            if(EnemyLayerMask != 0)
                result.Add(nameof(EnemyLayerMask), EnemyLayerMask);

            return result;
        }
        
        public float GetValueByName(string fieldName)
        {
            var value = GetValueByNameNullable(fieldName);

            if (value is not null)
            {
                return value.Value;
            }
            else
            {
                Debug.LogError($"Value in {nameof(RangedWeaponStatistics)} called {fieldName} doesn't exist!");
                return float.MinValue;
            }
        }
        
        public float? GetValueByNameNullable(string fieldName)
        {
            return fieldName switch
            {
                nameof(WaitingTimeForNextShoot) => WaitingTimeForNextShoot,
                nameof(BulletType) => (float)BulletType,
                nameof(SpreadType) => (float)SpreadType,
                nameof(Spread) => Spread,
                nameof(SpreadStep) => SpreadStep,
                nameof(SpreadThreshold) => SpreadThreshold,
                nameof(SpreadResetThreshold) => SpreadResetThreshold,
                nameof(ProjectilesPerShot) => ProjectilesPerShot,
                nameof(MagazineSize) => MagazineSize,
                nameof(Range) => Range,
                nameof(LoudnessRange) => LoudnessRange,
                nameof(EnemyLayerMask) => (float)EnemyLayerMask,
                _ => null
            };
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
                   Range != 0 ||
                   LoudnessRange != 0 ||
                   EnemyLayerMask != 0;
        }
    }
}
