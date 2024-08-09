using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Weapons.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUpgradeHolder", menuName = "Project BLAKE/Weapon Upgrade Holder")]
    public class WeaponUpgradeHolder : ScriptableObject
    {
        public List<WeaponUpgradeSo> melee;
        public List<WeaponUpgradeSo> ranged;
    }
}