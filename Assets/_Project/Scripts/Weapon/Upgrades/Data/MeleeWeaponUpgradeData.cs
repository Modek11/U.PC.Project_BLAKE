using System;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class MeleeWeaponUpgradeData : IWeaponUpgradeData
    {
        public WeaponUpgradeRarityEnum weaponUpgradeRarity;
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}