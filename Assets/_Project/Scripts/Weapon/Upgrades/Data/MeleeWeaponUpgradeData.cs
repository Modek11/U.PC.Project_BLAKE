using System;
using _Project.Scripts.Weapon.Statistics;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class MeleeWeaponUpgradeData : WeaponUpgradeData
    {
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}