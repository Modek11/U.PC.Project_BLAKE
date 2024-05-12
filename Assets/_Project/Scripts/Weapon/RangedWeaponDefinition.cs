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

        [Tooltip("Declares type of spread")] 
        public SpreadType SpreadType = SpreadType.Undefined;
        
        [Tooltip("Declares range of bullet spawn")]
        public float Spread = 10f;
        
        [Tooltip("Declares in seconds how fast spread threshold should reset. Used only for: (default: -1) \n" +
                 "   -NoSpreadThenStatic\n" +
                 "   -GraduallyIncrease")]
        public float SpreadThreshold = -1f;
        
        [Tooltip("Declares how many projectiles should be instantiated in one shot")]
        public int ProjectilesPerShot = 1;

        public int MagazineSize;
        
        public float Range;
    }
}