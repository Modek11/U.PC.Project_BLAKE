using System;

namespace _Project.Scripts.Weapon.Upgrades
{
    [Serializable]
    public class MeleeWeaponUpgradeData
    {
        public string displayedDescription;
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}