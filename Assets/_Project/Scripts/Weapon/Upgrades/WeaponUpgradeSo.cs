using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponUpgradeSo")]
    public class WeaponUpgradeSo : ScriptableObject
    {
        public WeaponDefinition weaponDefinition;
        public List<RangedWeaponUpgradeData> rangedUpgradeData;
        public List<MeleeWeaponUpgradeData> meleeUpgradeData;
    }
}