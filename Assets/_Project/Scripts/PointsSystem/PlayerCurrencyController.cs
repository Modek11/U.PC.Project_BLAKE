using _Project.Scripts.PointsSystem.ComboSystem;
using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public class PlayerCurrencyController : MonoBehaviour
    {
        [SerializeField] private ComboController comboController;
        public delegate void AddedPointsDeleagte(float addedPoints);
        public event AddedPointsDeleagte onAddPoints;
        
        private float points = 0;

        [SerializeField]
        private float lostPointOnDeathPercentage = 20;
        [SerializeField]
        private float deathPointsModifier = 0;
        public float Points => points;

        public void RegisterEnemyDeath(int pointsForKill)
        {
            float pointsToAdd = pointsForKill * comboController.ComboCounter;
            points += pointsToAdd;
            onAddPoints?.Invoke(pointsToAdd);
        }

        public void AddPoints(float points)
        {
            this.points += points;
        }

        public void RemovePoints(float points)
        {
            this.points -= points;
        }

        public void LosePointsOnDeath()
        {
            var value = Mathf.Max((lostPointOnDeathPercentage + deathPointsModifier) / 100f, 0f);
            RemovePoints(Mathf.Round(points * value));
            EnemyDeathMediator.Instance.Refresh();
        }

        public void AddDeathModifierPercentage(float percentage)
        {
            deathPointsModifier += percentage;
        }
    }
}
