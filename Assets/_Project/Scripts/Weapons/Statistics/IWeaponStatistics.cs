using System.Collections.Generic;

namespace _Project.Scripts.Weapons.Statistics
{
    public interface IWeaponStatistics
    {
        public Dictionary<string, float> GetNonZeroFields();
        public float GetValueByName(string fieldName);
        public bool IsNullOrEmpty();
    }
}