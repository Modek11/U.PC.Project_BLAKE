using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.Patterns;
using _Project.Scripts.Weapon.Upgrades.UI;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class WeaponUpgradeManager : Singleton<WeaponUpgradeManager>
    {
        private const int NUMBER_OF_CARDS = 4;
        
        [SerializeField] private WeaponUpgradeHolder weaponUpgradeHolder;
        [SerializeField] private WeaponUpgradeCardUI upgradeCardPrefab;
        [SerializeField] private WeaponStatisticUpgradeUI statisticUpgradePrefab;
        
        private WeaponUpgradeUIManager weaponUpgradeUIManager;
        private bool isUpgradeAvailable = true;

        public bool IsUpgradeAvailable
        {
            get => isUpgradeAvailable;
            private set
            {
                isUpgradeAvailable = value;
                UpgradeAvailabilityChanged();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            ReferenceManager.WeaponUpgradeManager = this;
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
                CreateCards();
                CreateCards();
                CreateCards();
                CreateCards();
            }
            else
            {
                GameHandler.Instance.OpenWeaponUpgradesCanvas();
            }
        }

        private void CreateCards()
        {
            var randomWeaponNumber = Random.Range(0, weaponUpgradeHolder.ranged.Count);
            var drawnWeapon = weaponUpgradeHolder.ranged[randomWeaponNumber];
            var randomStatisticNumber = Random.Range(0, drawnWeapon.rangedUpgradeData.Count);
            var upgradeData = drawnWeapon.rangedUpgradeData[randomStatisticNumber];
            
            var weaponName = drawnWeapon.weaponDefinition.WeaponName;
            var weaponRarity = upgradeData.weaponUpgradeRarity;
            var weaponStatistics = upgradeData.rangedWeaponStatistics.GetNonZeroFields();
            
            
            CreateCard(weaponName, weaponRarity, weaponStatistics);
        }

        private void CreateCard(string weaponName, WeaponUpgradeRarityEnum weaponRarity, Dictionary<string,float> weaponStatistics)
        {
            var instantiatedCard = weaponUpgradeUIManager.CreateNewUpgradeCard(upgradeCardPrefab);
            instantiatedCard.SetupCard(weaponName, weaponRarity);
            

            foreach (var statistic in weaponStatistics)
            {
                CreateStatistic(instantiatedCard, statistic.Key, statistic.Value.ToString());
            }
        }

        private void CreateStatistic(WeaponUpgradeCardUI instantiatedCard, string upgradeName, string upgradeValue)
        {
            var instantiatedStatistic = instantiatedCard.CreateNewUpgradeStatistic(statisticUpgradePrefab);
            
            instantiatedStatistic.SetupStatistic(upgradeName, upgradeValue);
        }
        
        private void RerollUpgrades()
        {
            Debug.LogError("Reroll is not implemented");
        }
        
        private void UpgradeAvailabilityChanged()
        {
            if (!isUpgradeAvailable)
            {
            }
        }
        
    }
}
