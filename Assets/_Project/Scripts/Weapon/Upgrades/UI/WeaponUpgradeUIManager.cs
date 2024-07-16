using _Project.Scripts.Weapon.Upgrades.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Weapon.Upgrades.UI
{
    public class WeaponUpgradeUIManager : MonoBehaviour, IWeaponUpgradeData
    {
        [SerializeField] private Transform upgradeCardParent;
        [SerializeField] private Button rerollButton;

        public Button RerollButton => rerollButton;

        public WeaponUpgradeCardUI CreateNewUpgradeCard(WeaponUpgradeCardUI upgradeCardPrefab)
        {
            return Instantiate(upgradeCardPrefab, upgradeCardParent, false);
        }

        public void DestroyCards()
        {
            for (var i = 0; i < upgradeCardParent.childCount; i++)
            {
                var card = upgradeCardParent.GetChild(i);
                Destroy(card.gameObject);
            }
        }
    }
}
