using System;
using _Project.Scripts.Weapons.Statistics;

namespace _Project.Scripts.Weapons.Upgrades.Data
{
    [Serializable]
    public class MeleeWeaponUpgradeData : WeaponUpgradeData
    {
        public MeleeWeaponStatistics meleeWeaponStatistics;
    }
}