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

        private Dictionary<WeaponUpgradeCardUI, RangedWeaponStatistics> dictionaryOfUpgrades = new ();
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
                
                RerollUpgrades();
                weaponUpgradeUIManager.RerollButton.onClick.AddListener(() => RerollUpgrades(true));
            }
            else
            {
                GameHandler.Instance.OpenWeaponUpgradesCanvas();
            }
        }

        private void CreateUpgrade()
        {
            var randomWeaponNumber = Random.Range(0, weaponUpgradeHolder.ranged.Count);
            var drawnWeapon = weaponUpgradeHolder.ranged[randomWeaponNumber];
            var randomStatisticNumber = Random.Range(0, drawnWeapon.rangedUpgradeData.Count);
            var upgradeData = drawnWeapon.rangedUpgradeData[randomStatisticNumber];
            
            var weaponName = drawnWeapon.weaponDefinition.WeaponName;
            var weaponRarity = upgradeData.weaponUpgradeRarity;
            
            var weaponStatistics = upgradeData.rangedWeaponStatistics;
            
            
            CreateCard(weaponName, weaponRarity, weaponStatistics);
        }

        private void CreateCard(string weaponName, WeaponUpgradeRarityEnum weaponRarity, RangedWeaponStatistics weaponStatistics)
        {
            var instantiatedCard = weaponUpgradeUIManager.CreateNewUpgradeCard(upgradeCardPrefab);
            instantiatedCard.SetupCard(weaponName, weaponRarity);
            
            dictionaryOfUpgrades.Add(instantiatedCard, weaponStatistics);
            
            instantiatedCard.BuyButton.onClick.AddListener(() => ApplyUpgrades(instantiatedCard));
            
            foreach (var statistic in weaponStatistics.GetNonZeroFields())
            {
                CreateStatistic(instantiatedCard, statistic.Key, statistic.Value.ToString());
            }
        }

        private void CreateStatistic(WeaponUpgradeCardUI instantiatedCard, string upgradeName, string upgradeValue)
        {
            var instantiatedStatistic = instantiatedCard.CreateNewUpgradeStatistic(statisticUpgradePrefab);
            
            instantiatedStatistic.SetupStatistic(upgradeName, upgradeValue);
        }
        
        private void ApplyUpgrades(WeaponUpgradeCardUI card)
        {
            
            if (dictionaryOfUpgrades.TryGetValue(card, out var statistics))
            {
                ReferenceManager.BlakeHeroCharacter.GetComponent<PlayerWeaponUpgradeManager>()
                    .AddWeaponUpgrade(card.WeaponName.text, statistics);
            }
            else
            {
                Debug.LogError("No statistics found!");
            }
        }
        
        private void RerollUpgrades(bool rerolledByPlayer = false)
        {
            weaponUpgradeUIManager.DestroyCards();
            dictionaryOfUpgrades.Clear();
            CreateUpgrade();
            CreateUpgrade();
            CreateUpgrade();
            CreateUpgrade();

            if (rerolledByPlayer)
            {
            }
        }
        
        private void UpgradeAvailabilityChanged()
        {
            if (!isUpgradeAvailable)
            {
            }
        }
        
    }
}
