using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.UI.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Weapons.Upgrades.UI
{
    public class WeaponUpgradeUIManager : MonoBehaviour
    {
        private const string REROLL_BUTTON_PREFIX = "Reroll:";

        [SerializeField] private PausedGameCanvasController pausedGameCanvasController;
        [SerializeField] private Transform upgradeCardParent;
        [SerializeField] private Button rerollButton;
        [SerializeField] private TextMeshProUGUI rerollButtonText;
        
        private float rerollCost = float.MaxValue;

        public Button RerollButton => rerollButton;
        
        private void OnEnable()
        {
            SwitchRerollButtonInteractable();
        }

        public void CloseUI()
        {
            pausedGameCanvasController.CloseAllCanvasAndUnpause();
        }

        public void UpdateRerollButton(float cost)
        {
            rerollCost = cost;
            rerollButtonText.text = $"{REROLL_BUTTON_PREFIX} {rerollCost}";
            SwitchRerollButtonInteractable();
        }
        
        public WeaponUpgradeCardUI CreateNewUpgradeCard(WeaponUpgradeCardUI upgradeCardPrefab)
        {
            return Instantiate(upgradeCardPrefab, upgradeCardParent, false);
        }
        
        private void SwitchRerollButtonInteractable()
        {
            var isEnoughPoints =
                ReferenceManager.PlayerCurrencyController.HasPlayerEnoughPoints(rerollCost);
            rerollButton.interactable = isEnoughPoints;
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
