using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "Melee Weapon definition", menuName = "Project BLAKE/Melee Weapon")]
    public class MeleeWeaponDefinition : WeaponDefinition
    {
        [Space(10), Header("Weapon statistics")] 
        
        public float SpereCastRadius;
        
        public float MaxDistance;
        
        public LayerMask LayerMask;
    }
}