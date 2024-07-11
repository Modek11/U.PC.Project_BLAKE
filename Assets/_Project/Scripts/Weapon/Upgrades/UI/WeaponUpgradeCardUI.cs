using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponUpgradeCardUI : MonoBehaviour
    {
        [SerializeField] private Transform statisticParent;
        [SerializeField] private Image borderImage;
        [SerializeField] private Button buyButton;

        public Color BorderColor
        {
            get => borderImage.color;
            set => borderImage.color = value;
        }
        public Button BuyButton => buyButton;

        public WeaponStatisticUpgradeUI CreateNewUpgradeStatistic(WeaponStatisticUpgradeUI weaponStatisticUpgradeUI)
        {
            return Instantiate(weaponStatisticUpgradeUI, statisticParent, false);
        }
    }
}
