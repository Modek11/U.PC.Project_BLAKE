using System;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class RangedWeaponUpgradeData : IWeaponUpgradeData
    {
        public string displayedDescription;
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}