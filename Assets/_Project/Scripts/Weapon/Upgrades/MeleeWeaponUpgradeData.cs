using System;

namespace _Project.Scripts.Weapon.Upgrades
{
    [Serializable]
    public class MeleeWeaponUpgradeData
    {
        public WeaponUpgradeTierEnum weaponUpgradeTier;
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}