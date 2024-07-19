using TMPro;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponStatisticUpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeValue;
        
        public void SetupStatistic(string upgradeName, float upgradeValue)
        {
            this.upgradeName.text = HelperUtils.SplitCamelCase(upgradeName);
            this.upgradeValue.text = upgradeValue.ToString();
            this.upgradeValue.color = upgradeValue >= 0 ? Color.green : Color.red;
        }
    }
}