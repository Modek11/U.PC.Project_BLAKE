using System;
using System.Collections.Generic;
using _Project.Scripts.Weapon.Definition;
using _Project.Scripts.Weapon.Statistics;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class PlayerWeaponUpgradeManager : MonoBehaviour
    {
        [SerializeField] private WeaponDefinitionHolder weaponDefinitionHolder;

        private Dictionary<WeaponDefinition, RangedWeaponStatistics> rangedWeaponsUpgrades = new ();
        private Dictionary<WeaponDefinition, MeleeWeaponStatistics> meleeWeaponsUpgrades = new ();
        
        public event Action<WeaponDefinition, IWeaponStatistics> OnWeaponUpgradeChanged;
        
        private void Start()
        {
            PrepareClearDictionaries();
            GetComponent<WeaponsManager>().OnPlayerPickupWeaponEvent += OnPlayerPickupWeaponEvent;
        }

        private void PrepareClearDictionaries()
        {
            meleeWeaponsUpgrades.Clear();
            meleeWeaponsUpgrades.Add(GetComponent<WeaponsManager>().defaultWeapon, new MeleeWeaponStatistics());
            
            rangedWeaponsUpgrades.Clear();
            foreach (var weapon in weaponDefinitionHolder.ranged)
            {
                rangedWeaponsUpgrades.Add(weapon, new RangedWeaponStatistics());
            }
        }
        
        private void OnPlayerPickupWeaponEvent(Weapon weapon)
        {
            if (weapon is RangedWeapon rangedWeapon)
            {
                var definition = rangedWeapon.WeaponDefinition;
                if(rangedWeaponsUpgrades.TryGetValue(definition, out var statistics))
                {
                    weapon.CalculateWeaponStatsWithUpgrades(definition, statistics);
                }
            }
            else if (weapon is MeleeWeapon meleeWeapon)
            {
                var definition = meleeWeapon.WeaponDefinition;
                if(meleeWeaponsUpgrades.TryGetValue(definition, out var statistics))
                {
                    weapon.CalculateWeaponStatsWithUpgrades(definition, statistics);
                }
            }
            else
            {
                Debug.LogError($"Weapon is not valid!");
            }
        }

        public void AddWeaponUpgrade(string weaponName, IWeaponStatistics weaponStatistics)
        {
            if (weaponStatistics is RangedWeaponStatistics rangedStatistics)
            {
                AddRangedWeaponUpgrade(weaponName, rangedStatistics);
            }
            else if (weaponStatistics is MeleeWeaponStatistics meleeStatistics)
            {
                AddMeleeWeaponUpgrade(weaponName, meleeStatistics);
            }
            else
            {
                Debug.LogError("Wrong weapon statistics!");
            }
        }

        private void AddRangedWeaponUpgrade(string weaponName, RangedWeaponStatistics weaponStatistics)
        {
            var passedWeaponName = weaponName.ToLower().Replace(" ", "");
            foreach (var (weaponDefinition, statistics) in rangedWeaponsUpgrades)
            {
                var weaponDefinitionName = weaponDefinition.WeaponName.ToLower().Replace(" ", "");
                if (passedWeaponName == weaponDefinitionName)
                {
                    var upgradedValues = statistics + weaponStatistics;
                    rangedWeaponsUpgrades[weaponDefinition] = upgradedValues;
                    OnWeaponUpgradeChanged?.Invoke(weaponDefinition, upgradedValues);
                    return;
                }
            }
            
            Debug.LogError($"Missing weapon called {weaponName}");
        }
        
        private void AddMeleeWeaponUpgrade(string weaponName, MeleeWeaponStatistics weaponStatistics)
        {
            var passedWeaponName = weaponName.ToLower().Replace(" ", "");
            foreach (var (weaponDefinition, statistics) in meleeWeaponsUpgrades)
            {
                var weaponDefinitionName = weaponDefinition.WeaponName.ToLower().Replace(" ", "");
                if (passedWeaponName == weaponDefinitionName)
                {
                    var upgradedValues = statistics + weaponStatistics;
                    meleeWeaponsUpgrades[weaponDefinition] = upgradedValues;
                    OnWeaponUpgradeChanged?.Invoke(weaponDefinition, upgradedValues);
                    return;
                }
            }
            
            Debug.LogError($"Missing weapon called {weaponName}");
        }

        public IWeaponStatistics GetCurrentWeaponStatistics(WeaponDefinition weaponDefinition)
        {
            switch (weaponDefinition)
            {
                case RangedWeaponDefinition rangedWeaponDefinition:
                    return GetCurrentWeaponStatistics(rangedWeaponDefinition);
                case MeleeWeaponDefinition meleeWeaponDefinition:
                    return GetCurrentWeaponStatistics(meleeWeaponDefinition);
                default:
                    Debug.LogError("STATISTICS ARE EMPTY");
                    return null;
            }
        }
        
        private RangedWeaponStatistics? GetCurrentWeaponStatistics(RangedWeaponDefinition weaponDefinition)
        {
            var rangedIndex = weaponDefinitionHolder.ranged.IndexOf(weaponDefinition);
            var definition = weaponDefinitionHolder.ranged[rangedIndex] as RangedWeaponDefinition;
            var baseStatistics = definition.GetWeaponStatistics();

            rangedWeaponsUpgrades.TryGetValue(weaponDefinition, out var upgradedStatistics);
            
            return baseStatistics + upgradedStatistics;
        }
        
        private MeleeWeaponStatistics? GetCurrentWeaponStatistics(MeleeWeaponDefinition weaponDefinition)
        {
            var meleeIndex = weaponDefinitionHolder.melee.IndexOf(weaponDefinition);
            var definition = weaponDefinitionHolder.melee[meleeIndex] as MeleeWeaponDefinition;
            var baseStatistics = definition.GetWeaponStatistics();
            
            meleeWeaponsUpgrades.TryGetValue(weaponDefinition, out var upgradedStatistics);
            
            return baseStatistics + upgradedStatistics;
        }
    }
}
