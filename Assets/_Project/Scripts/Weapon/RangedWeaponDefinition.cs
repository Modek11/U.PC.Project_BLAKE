using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "Ranged Weapon definition", menuName = "Project BLAKE/Ranged Weapon")]
    public class RangedWeaponDefinition : WeaponDefinition
    {
        [Space(10), Header("Weapon statistics")] 
        
        public BasicBullet BasicBullet;
        
        [Tooltip("Declares how often player can shoot")]
        public float FireDelayTime = 0.4f;
        
        [Tooltip("Declares range of bullet spawn")]
        public float Spread = 10f;
        
        [Tooltip("Declares how fast should be character to use spread during shot")]
        public float SpreadMinMagnitude = 0f;
        
        [Tooltip("Declares how many projectiles should be instantiated in one shot")]
        public int ProjectilesPerShot = 1;

        public int MagazineSize;
        
        public float Range;
    }
}