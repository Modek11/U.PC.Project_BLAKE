using System;
using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.Patterns;
using _Project.Scripts.Weapon.Upgrades.Data;
using _Project.Scripts.Weapon.Upgrades.UI;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class WeaponUpgradeManager : Singleton<WeaponUpgradeManager>
    {
        [SerializeField] private WeaponUpgradeHolder weaponUpgradeHolder;
        [SerializeField] private WeaponUpgradeCardUI upgradeCardPrefab;
        [SerializeField] private WeaponStatisticUpgradeUI statisticUpgradePrefab;
        
        private WeaponUpgradeUIManager weaponUpgradeUIManager;
        
        public bool IsUpgradeAvailable { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            ReferenceManager.WeaponUpgradeManager = this;

            IsUpgradeAvailable = true;
        }

        public void TryShowWeaponUpgrades()
        {
            if (!IsUpgradeAvailable)
            {
                return;
            }

            if (weaponUpgradeUIManager is null)
            { 
                weaponUpgradeUIManager = GameHandler.Instance.OpenWeaponUpgradesCanvas().GetComponent<WeaponUpgradeUIManager>();
                weaponUpgradeUIManager.RerollButton.onClick.AddListener(RerollUpgrades);
                CreateCard();
            }
            else
            {
                GameHandler.Instance.OpenWeaponUpgradesCanvas();
            }
        }

        private void CreateCard()
        {
            var instantiatedCard = weaponUpgradeUIManager.CreateNewUpgradeCard(upgradeCardPrefab);
            instantiatedCard.BorderColor = Color.yellow;

            CreateStatistic(instantiatedCard);
        }

        private void CreateStatistic(WeaponUpgradeCardUI instantiatedCard)
        {
            var instantiatedStatistic = instantiatedCard.CreateNewUpgradeStatistic(statisticUpgradePrefab);

            instantiatedStatistic.UpgradeName.text = "Gun name";
            instantiatedStatistic.UpgradeValue.text = "2 + 2 = 5";
        }

        private void GetAvailableUpgrades(IWeaponUpgradeData weaponUpgradeData)
        {
            if (weaponUpgradeData is RangedWeaponUpgradeData rangedWeaponUpgradeData)
            {
            }
            else if (weaponUpgradeData is MeleeWeaponUpgradeData meleeWeaponUpgradeData)
            {
            }
            else
            {
                throw new NotImplementedException("Passed data is not melee either ranged");
            }
        }
        
        private void RerollUpgrades()
        {
            Debug.LogError("Reroll is not implemented");
        }
        
    }
}
