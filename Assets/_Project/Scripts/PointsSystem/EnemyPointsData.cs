using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public class EnemyPointsData : MonoBehaviour
    {
        [SerializeField]
        private EnemyCharacter enemyCharacter;
        
        [SerializeField] 
        private EnemyTypeEnum enemyTypeEnum;
        
        [SerializeField, HideInInspector] 
        private int pointsForKill;
        
        public EnemyTypeEnum EnemyTypeEnum => enemyTypeEnum;

        private void Awake()
        {
            enemyCharacter.onDeath += EnemyCharacterOnDeath;
        }

        public void SetPointsForKill(int value)
        {
            pointsForKill = value;
        }

        private void EnemyCharacterOnDeath()
        {
            EnemyDeathMediator.Instance.RegisterEnemyDeath(pointsForKill, enemyTypeEnum);
            UnregisterEvent();
        }

        private void UnregisterEvent()
        {
            enemyCharacter.onDeath -= EnemyCharacterOnDeath;
        }

        private void OnDestroy()
        {
            UnregisterEvent();
        }
        
    }
}
