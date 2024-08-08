using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Weapon.Statistics
{
    [Serializable]
    public struct MeleeWeaponStatistics : IWeaponStatistics
    {
        public MeleeWeaponStatistics(float attackDelayTime, float sphereCastRadius, 
            int maxSpreadRange, LayerMask enemyLayerMask, int maxNumberOfEnemies)
        {
            AttackDelayTime = attackDelayTime;
            SphereCastRadius = sphereCastRadius;
            MaxSpreadRange = maxSpreadRange;
            EnemyLayerMask = enemyLayerMask;
            MaxNumberOfEnemies = maxNumberOfEnemies;
        }

        public float AttackDelayTime;
        public float SphereCastRadius;
        public int MaxSpreadRange;
        public LayerMask EnemyLayerMask;
        public int MaxNumberOfEnemies;
        
        public static MeleeWeaponStatistics operator +(MeleeWeaponStatistics a, MeleeWeaponStatistics b)
        {
            return new MeleeWeaponStatistics(
                a.AttackDelayTime + b.AttackDelayTime,
                a.SphereCastRadius + b.SphereCastRadius,
                a.MaxSpreadRange + b.MaxSpreadRange,
                a.EnemyLayerMask, // Assuming LayerMask should remain unchanged
                a.MaxNumberOfEnemies + b.MaxNumberOfEnemies
            );
        }
        
        public Dictionary<string, float> GetNonZeroFields()
        {
            var result = new Dictionary<string, float>();

            if (AttackDelayTime != 0)
                result.Add(nameof(AttackDelayTime), AttackDelayTime);

            if (SphereCastRadius != 0)
                result.Add(nameof(SphereCastRadius), SphereCastRadius);

            if (MaxSpreadRange != 0)
                result.Add(nameof(MaxSpreadRange), MaxSpreadRange);

            if (EnemyLayerMask != 0)
                result.Add(nameof(EnemyLayerMask), EnemyLayerMask.value);

            if (MaxNumberOfEnemies != 0)
                result.Add(nameof(MaxNumberOfEnemies), MaxNumberOfEnemies);

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
                Debug.LogError($"Value in {nameof(MeleeWeaponStatistics)} called {fieldName} doesn't exist!");
                return float.MinValue;
            }
        }
        
        public float? GetValueByNameNullable(string fieldName)
        {
            return fieldName switch
            {
                nameof(AttackDelayTime) => AttackDelayTime,
                nameof(SphereCastRadius) => SphereCastRadius,
                nameof(MaxSpreadRange) => MaxSpreadRange,
                nameof(EnemyLayerMask) => EnemyLayerMask.value,
                nameof(MaxNumberOfEnemies) => MaxNumberOfEnemies,
                _ => null
            };
        }

        
        public bool IsNullOrEmpty()
        {
            return AttackDelayTime != 0 ||
                   SphereCastRadius != 0 ||
                   MaxSpreadRange != 0 ||
                   EnemyLayerMask != 0 ||
                   MaxNumberOfEnemies != 0;
        }

    }
}