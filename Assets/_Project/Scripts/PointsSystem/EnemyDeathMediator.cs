using System;
using _Project.Scripts.Patterns;
using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public struct ComboAndPointsValues
    {
        public ComboAndPointsValues(float points, float comboCounter, int killsCounter)
        {
            Points = points;
            ComboCounter = comboCounter;
            KillsCounter = killsCounter;
        }
        
        public float Points;
        public float ComboCounter;
        public int KillsCounter;
    }
    
    public class EnemyDeathMediator : Singleton<EnemyDeathMediator>
    {
        [SerializeField] private PlayerCurrencyController playerCurrencyController;
        [SerializeField] private ComboController comboController;

        private float points => playerCurrencyController.Points;
        private float comboCounter => comboController.ComboCounter;
        private int killsCounter => comboController.KillsCounter;
        
        public event Action<ComboAndPointsValues> OnRegisteredEnemyDeath;

        public ComboController ComboController => comboController;
        
        public void RegisterEnemyDeath(int pointsForKill, EnemyTypeEnum enemyTypeEnum)
        {
            playerCurrencyController.RegisterEnemyDeath(pointsForKill);
            comboController.RegisterEnemyDeath();
            
            OnRegisteredEnemyDeath?.Invoke(new ComboAndPointsValues(points, comboCounter, killsCounter));
        }
    }
}
