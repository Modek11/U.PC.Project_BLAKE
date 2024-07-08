
using System;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [Serializable]
    public struct MeleeWeaponStatistics
    {
        public MeleeWeaponStatistics(float attackDelayTime, float sphereCastRadius, 
            int maxSpreadRange, LayerMask layerMask, int maxNumberOfEnemies)
        {
            AttackDelayTime = attackDelayTime;
            SphereCastRadius = sphereCastRadius;
            MaxSpreadRange = maxSpreadRange;
            LayerMask = layerMask;
            MaxNumberOfEnemies = maxNumberOfEnemies;
        }

        public float AttackDelayTime;
        public float SphereCastRadius;
        public int MaxSpreadRange;
        public LayerMask LayerMask;
        public int MaxNumberOfEnemies;
    }
}