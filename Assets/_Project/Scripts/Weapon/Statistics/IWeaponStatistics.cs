using System.Collections.Generic;

namespace _Project.Scripts.Weapon.Statistics
{
    public interface IWeaponStatistics
    {
        public Dictionary<string, float> GetNonZeroFields();
        public float GetValueByName(string fieldName);
        public bool IsNullOrEmpty();
    }
}