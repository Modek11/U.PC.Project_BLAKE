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
    }
}
