using System;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class MeleeWeaponUpgradeData : IWeaponUpgradeData
    {
        public string displayedDescription;
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}