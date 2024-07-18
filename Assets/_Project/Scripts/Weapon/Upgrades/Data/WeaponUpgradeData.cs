using System;
using _Project.Scripts.Weapon.Definition;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public abstract class WeaponUpgradeData
    {
        public int UpgradeCost;
        public WeaponDefinition WeaponDefinition;
        public WeaponUpgradeRarityEnum WeaponUpgradeRarity;
    }
}