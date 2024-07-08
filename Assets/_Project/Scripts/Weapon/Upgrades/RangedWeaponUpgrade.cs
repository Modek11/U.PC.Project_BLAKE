using System;

namespace _Project.Scripts.Weapon.Upgrades
{
    [Serializable]
    public class RangedWeaponUpgradeData
    {
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}