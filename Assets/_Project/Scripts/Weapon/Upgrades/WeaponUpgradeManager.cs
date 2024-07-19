using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.Patterns;
using _Project.Scripts.Weapon.Statistics;
using _Project.Scripts.Weapon.Upgrades.Data;
using _Project.Scripts.Weapon.Upgrades.UI;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class WeaponUpgradeManager : Singleton<WeaponUpgradeManager>
    {
        private const int NUMBER_OF_CARDS = 4;
        private const int UPGRADES_REROLL_MAX_TRIES = 100;
        
        [SerializeField] private WeaponUpgradeHolder weaponUpgradeHolder;
        [SerializeField] private WeaponUpgradeCardUI upgradeCardPrefab;
        [SerializeField] private WeaponStatisticUpgradeUI statisticUpgradePrefab;
        [SerializeField] private float baseMeleeWeaponDrawChance;
        [SerializeField] private int baseRerollCost;
        [SerializeField] private float baseRerollMultiplier;

        private Dictionary<WeaponUpgradeData, IWeaponStatistics> dictionaryOfUpgrades = new ();
        private WeaponUpgradeUIManager weaponUpgradeUIManager;
        private bool isUpgradeAvailable = true;

        private float currentMeleeWeaponDrawChance;
        private int currentRerollCost;
        private float currentRerollMultiplier;

        public bool IsUpgradeAvailable => isUpgradeAvailable;

        protected override void Awake()
        {
            base.Awake();

            ReferenceManager.WeaponUpgradeManager = this;
            currentMeleeWeaponDrawChance = baseMeleeWeaponDrawChance;
            currentRerollCost = baseRerollCost;
            currentRerollMultiplier = baseRerollMultiplier;
            
            ReferenceManager.LevelHandler.onNextLevel += OnNextLevel;
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
        
        private void RerollUpgrades(bool rerolledByPlayer = false)
        {
            if (rerolledByPlayer)
            {
                ReferenceManager.PlayerCurrencyController.RemovePoints(currentRerollCost);
                currentRerollCost = (int)(currentRerollCost * currentRerollMultiplier);
            }
            
            weaponUpgradeUIManager.UpdateRerollButton(currentRerollCost);

            DestroyCards();

            for (var i = 0; i < NUMBER_OF_CARDS; i++)
            {
                CreateUpgrade();
            }
        }

        private void CreateUpgrade()
        {
            for (var i = 0; i < UPGRADES_REROLL_MAX_TRIES; i++)
            {
                var upgradeData = GetUpgradeData();
                
                if (!dictionaryOfUpgrades.TryAdd(upgradeData.Item1, upgradeData.Item2))
                {
                    continue;
                }
                
                CreateCard(upgradeData.Item1, upgradeData.Item2);
                return;
            }
            
            Debug.LogError($"Cannot find available upgrade!");
        }

        private (WeaponUpgradeData, IWeaponStatistics) GetUpgradeData()
        {
            WeaponUpgradeData upgradeData;
            IWeaponStatistics weaponStatistics;
            
            if(Random.value > currentMeleeWeaponDrawChance)
            {
                var randomWeaponNumber = Random.Range(0, weaponUpgradeHolder.ranged.Count);
                var drawnWeapon = weaponUpgradeHolder.ranged[randomWeaponNumber];
                var randomStatisticNumber = Random.Range(0, drawnWeapon.rangedUpgradeData.Count);
                upgradeData = drawnWeapon.rangedUpgradeData[randomStatisticNumber];

                var rangedUpgradeData = (RangedWeaponUpgradeData)upgradeData;
                weaponStatistics = rangedUpgradeData.rangedWeaponStatistics;
            }
            else
            {
                var randomWeaponNumber = Random.Range(0, weaponUpgradeHolder.melee.Count);
                var drawnWeapon = weaponUpgradeHolder.melee[randomWeaponNumber];
                var randomStatisticNumber = Random.Range(0, drawnWeapon.meleeUpgradeData.Count);
                upgradeData = drawnWeapon.meleeUpgradeData[randomStatisticNumber];

                var rangedUpgradeData = (MeleeWeaponUpgradeData)upgradeData;
                weaponStatistics = rangedUpgradeData.meleeWeaponStatistics;
            }
            
            return (upgradeData, weaponStatistics);
        }

        private void CreateCard(WeaponUpgradeData upgradeData, IWeaponStatistics weaponStatistics)
        {
            var instantiatedCard = weaponUpgradeUIManager.CreateNewUpgradeCard(upgradeCardPrefab);
            instantiatedCard.SetupCard(upgradeData, upgradeData.WeaponUpgradeRarity);
            
            instantiatedCard.BuyButton.onClick.AddListener(() => OnUpgradeBought(upgradeData));
            instantiatedCard.BuyButton.onClick.AddListener(() => weaponUpgradeUIManager.CloseUI());
            
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
        
        private void OnUpgradeBought(WeaponUpgradeData upgradeData)
        {
            if (dictionaryOfUpgrades.TryGetValue(upgradeData, out var statistics))
            {
                ReferenceManager.BlakeHeroCharacter.GetComponent<PlayerWeaponUpgradeManager>()
                    .AddWeaponUpgrade(upgradeData.WeaponDefinition.WeaponName, statistics);

                ReferenceManager.PlayerCurrencyController.RemovePoints(upgradeData.UpgradeCost);
                
                isUpgradeAvailable = false;
            }
            else
            {
                Debug.LogError("No statistics found!");
            }
        }
        
        private void OnNextLevel()
        {
            isUpgradeAvailable = true;
            ResetAllValues();
        }

        private void DestroyCards()
        {
            weaponUpgradeUIManager?.DestroyCards();
            dictionaryOfUpgrades.Clear();
        }

        private void ResetAllValues()
        {
            currentMeleeWeaponDrawChance = baseMeleeWeaponDrawChance;
            currentRerollCost = baseRerollCost;
            currentRerollMultiplier = baseRerollMultiplier;
            
            DestroyCards();
            weaponUpgradeUIManager.RerollButton.onClick.RemoveAllListeners();
            weaponUpgradeUIManager = null;
        }
        
    }
}
