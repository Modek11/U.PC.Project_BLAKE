using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponUpgradeCardUI : MonoBehaviour
    {
        private readonly Color COLOR_GRAY = Color.gray;
        private readonly Color COLOR_BLUE = Color.blue;
        private readonly Color COLOR_PURPLE = new Color(0.5f, 0f, 0.5f, 1f);
        private readonly Color COLOR_GOLD = new Color(0.831f, 0.686f, 0.216f, 1f);
        
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private Transform statisticParent;
        [SerializeField] private Image borderImage;
        [SerializeField] private Button buyButton;

        public Button BuyButton => buyButton;
        public TextMeshProUGUI WeaponName => weaponName;

        public WeaponStatisticUpgradeUI CreateNewUpgradeStatistic(WeaponStatisticUpgradeUI weaponStatisticUpgradeUI)
        {
            return Instantiate(weaponStatisticUpgradeUI, statisticParent, false);
        }

        public void SetupCard(string weaponName, WeaponUpgradeRarityEnum weaponRarity)
        {
            borderImage.color = GetColorByRarity(weaponRarity);
            this.weaponName.text = weaponName;
        }
        
        private Color GetColorByRarity(WeaponUpgradeRarityEnum weaponRarity)
        {
            switch (weaponRarity)
            {
                case WeaponUpgradeRarityEnum.Common:
                    return COLOR_GRAY;
                case WeaponUpgradeRarityEnum.Rare:
                    return COLOR_BLUE;
                case WeaponUpgradeRarityEnum.Epic:
                    return COLOR_PURPLE;
                case WeaponUpgradeRarityEnum.Legendary:
                    return COLOR_GOLD;
                case WeaponUpgradeRarityEnum.Undefined:
                default:
                    break;
            }
            
            Debug.LogError("Weapon rarity not assigned!");
            return Color.black;
        }
    }
}
