using TMPro;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponStatisticUpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeValue;
        
        public TextMeshProUGUI UpgradeName => upgradeName;
        public TextMeshProUGUI UpgradeValue => upgradeValue;
    }
}