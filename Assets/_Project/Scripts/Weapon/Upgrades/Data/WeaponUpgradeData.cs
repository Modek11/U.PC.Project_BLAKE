using System;

namespace _Project.Scripts.Weapon.Upgrades.Data
{
    [Serializable]
    public abstract class WeaponUpgradeData
    {
        public WeaponDefinition WeaponDefinition;
        public WeaponUpgradeRarityEnum WeaponUpgradeRarity;
    }
}