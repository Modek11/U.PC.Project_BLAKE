using System;
using _Project.Scripts.Weapons.Statistics;

namespace _Project.Scripts.Weapons.Upgrades.Data
{
    [Serializable]
    public class RangedWeaponUpgradeData : WeaponUpgradeData
    {
        public RangedWeaponStatistics rangedWeaponStatistics;
    }
}