using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    public class PlayerCurrencyController : MonoBehaviour
    {
        [SerializeField] private ComboController comboController;
        
        private float points = 0;

        public float Points => points;

        public void RegisterEnemyDeath(int pointsForKill)
        {
            points += pointsForKill * comboController.ComboCounter;
        }
    }
}
