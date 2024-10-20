using System;
using _Project.Scripts.Patterns;
using _Project.Scripts.PointsSystem.ComboSystem;
using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public struct ComboAndPointsValues
    {
        public ComboAndPointsValues(float points, float comboCounter, int killsCounter, bool shouldComboStart)
        {
            Points = points;
            ComboCounter = comboCounter;
            KillsCounter = killsCounter;
            ShouldComboStart = shouldComboStart;
        }
        
        public readonly float Points;
        public readonly float ComboCounter;
        public readonly int KillsCounter;
        public readonly bool ShouldComboStart;
    }
    
    public class EnemyDeathMediator : Singleton<EnemyDeathMediator>
    {
        [SerializeField] private ComboController comboController;

        private float points => PlayerCurrencyController.Points;
        private float comboCounter => comboController.ComboCounter;
        private int killsCounter => comboController.KillsCounter;
        private bool shouldComboStart => comboController.ShouldComboStart;
        
        public event Action<ComboAndPointsValues> OnRegisteredEnemyDeath;

        public ComboController ComboController => comboController;
        public PlayerCurrencyController PlayerCurrencyController => PlayerCurrencyController.Instance;
        public void RegisterEnemyDeath(int pointsForKill, EnemyTypeEnum enemyTypeEnum)
        {
            comboController.RegisterEnemyDeath();
            PlayerCurrencyController.RegisterEnemyDeath(pointsForKill);

            var values = new ComboAndPointsValues(points, comboCounter, killsCounter, shouldComboStart);
            OnRegisteredEnemyDeath?.Invoke(values);
        }

        public void Refresh()
        {
            var values = new ComboAndPointsValues(points, comboCounter, killsCounter, shouldComboStart);
            OnRegisteredEnemyDeath?.Invoke(values);
        }
    }
}
