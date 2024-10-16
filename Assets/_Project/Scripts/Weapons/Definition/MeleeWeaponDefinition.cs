using _Project.Scripts.Weapons.Statistics;
using UnityEngine;

namespace _Project.Scripts.Weapons.Definition
{
    [CreateAssetMenu(fileName = "Melee Weapon definition", menuName = "Project BLAKE/Melee Weapon")]
    public class MeleeWeaponDefinition : WeaponDefinition
    {
        [Space(10), Header("Weapon statistics")]

        public float AttackDelayTime;
        
        public float SpereCastRadius;
        
        [Tooltip("Value in degrees from front of object")]
        public int MaxSpreadRange;
        
        public LayerMask EnemyLayerMask;

        [Tooltip("Performance value: \n -higher value store more data \n -too low value can cause not damaging enemy")]
        public int MaxNumberOfEnemies;
        
        public MeleeWeaponStatistics GetWeaponStatistics()
        {
            return new MeleeWeaponStatistics(
                    AttackDelayTime,
                    SpereCastRadius,
                    MaxSpreadRange,
                    EnemyLayerMask,
                    MaxNumberOfEnemies);
        }
    }
}