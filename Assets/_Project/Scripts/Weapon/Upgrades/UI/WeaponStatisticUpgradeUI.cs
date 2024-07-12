using TMPro;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponStatisticUpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeValue;
        
        public void SetupStatistic(string upgradeName, string upgradeValue)
        {
            this.upgradeName.text = upgradeName;
            this.upgradeValue.text = upgradeValue;
        }
    }
}