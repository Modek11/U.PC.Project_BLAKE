using System;
using _Project.Scripts.Weapon.Statistics;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public class RangedWeaponUpgradeData : WeaponUpgradeData
    {
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}