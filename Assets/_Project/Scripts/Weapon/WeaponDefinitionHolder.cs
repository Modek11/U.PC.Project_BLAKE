using System.Collections.Generic;
using _Project.Scripts.Weapon.Definition;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "WeaponDefinitionHolder", menuName = "Project BLAKE/Weapon Definition Holder")]
    public class WeaponDefinitionHolder : ScriptableObject
    {
        public List<WeaponDefinition> melee;
        public List<WeaponDefinition> ranged;
    }
}

