using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public class PlayerCurrencyController : Singleton<PlayerCurrencyController>
    {
        [SerializeField] 
        private PointsDataSo pointsDataSo;
        
        private int points = 0;
        
        public PointsDataSo PointsDataSo { get; private set; }
        public int Points { get; private set; }

        public void AddPointsFromCharacterDeath(EnemyTypeEnum enemyTypeEnum)
        {
            
        }


    }
}
