using System;
using _Project.Scripts.Weapons.Definition;

namespace _Project.Scripts.Weapons.Upgrades.Data
{
    [Serializable]
    public abstract class WeaponUpgradeData
    {
        public int UpgradeCost;
        public WeaponDefinition WeaponDefinition;
        public WeaponUpgradeRarityEnum WeaponUpgradeRarity;
    }
}