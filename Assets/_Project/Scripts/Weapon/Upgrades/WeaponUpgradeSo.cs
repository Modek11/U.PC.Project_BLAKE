using System.Collections.Generic;
using _Project.Scripts.Weapon.Definition;
using _Project.Scripts.Weapon.Upgrades.Bullet;
using _Project.Scripts.Weapon.Upgrades.Data;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponUpgradeSo")]
    public class WeaponUpgradeSo : ScriptableObject
    {
        public WeaponDefinition weaponDefinition;
        public List<RangedWeaponUpgradeData> rangedUpgradeData;
        public List<MeleeWeaponUpgradeData> meleeUpgradeData;
        
        private void OnValidate()
        {
            foreach (var upgradeData in meleeUpgradeData)
            {
                upgradeData.WeaponDefinition = weaponDefinition;
                upgradeData.meleeWeaponStatistics.EnemyLayerMask = 0;
            }
            
            foreach (var upgradeData in rangedUpgradeData)
            {
                upgradeData.WeaponDefinition = weaponDefinition;
                upgradeData.rangedWeaponStatistics.BulletType = BulletType.Undefined;
                upgradeData.rangedWeaponStatistics.SpreadType = SpreadType.Undefined;
            }

            switch (weaponDefinition)
            {
                case RangedWeaponDefinition:
                    meleeUpgradeData.Clear();
                    break;
                case MeleeWeaponDefinition:
                    rangedUpgradeData.Clear();
                    break;
            }
        }
    }
}