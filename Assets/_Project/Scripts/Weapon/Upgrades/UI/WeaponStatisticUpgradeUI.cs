using TMPro;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponStatisticUpgradeUI : MonoBehaviour
    {
        private const string UPGRADE_ARROW = "->";
        
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeValue;
        
        public void SetupStatistic(string upgradeName, float currentValue, float upgradeValue)
        {
            this.upgradeName.text = HelperUtils.SplitCamelCase(upgradeName);
            this.upgradeValue.text = $"{currentValue} {UPGRADE_ARROW} {currentValue + upgradeValue}";
            this.upgradeValue.color = upgradeValue >= 0 ? Color.green : Color.red;
        }
    }
}