using System;

namespace _Project.Scripts.Weapon.Upgrades
{
    [Serializable]
    public class RangedWeaponUpgradeData
    {
        public string displayedDescription;
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}