using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.PointsSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PointsDataSo")]
    public class PointsDataSo : ScriptableObject
    {
        public List<PointsData> enemyData;
    }
}
