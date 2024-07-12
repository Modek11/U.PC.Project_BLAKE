using System;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class RangedWeaponUpgradeData : IWeaponUpgradeData
    {
        public WeaponUpgradeRarityEnum weaponUpgradeRarity;
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}