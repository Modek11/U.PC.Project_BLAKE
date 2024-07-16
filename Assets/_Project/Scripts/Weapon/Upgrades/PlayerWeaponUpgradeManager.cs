using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class PlayerWeaponUpgradeManager : MonoBehaviour
    {
        [SerializeField] private WeaponDefinitionHolder weaponDefinitionHolder;

        //[SerializeField] public Dictionary<WeaponDefinition, RangedWeaponStatistics> rangedWeaponsUpgrades = new Dictionary<WeaponDefinition, RangedWeaponStatistics>();
        //[SerializeField] private Dictionary<WeaponDefinition, MeleeWeaponStatistics> meleeWeaponsUpgrades = new Dictionary<WeaponDefinition, MeleeWeaponStatistics>();

        public event Action<WeaponDefinition, IWeaponStatistics> OnWeaponUpgradeChanged;
        
        [UDictionary.Split(30, 70)]
        public MeleeWeaponsUDictionary meleeWeaponsUpgrades;
        [Serializable]
        public class MeleeWeaponsUDictionary : UDictionary<WeaponDefinition, MeleeWeaponStatistics> { }
        
        [UDictionary.Split(30, 70)]
        public RangedWeaponsUDictionary rangedWeaponsUpgrades;
        [Serializable]
        public class RangedWeaponsUDictionary : UDictionary<WeaponDefinition, RangedWeaponStatistics> { }
        
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


        public RangedWeaponStatistics GetRangedWeaponStatistics(WeaponDefinition weaponDefinition)
        {
            if(rangedWeaponsUpgrades.TryGetValue(weaponDefinition, out var statistics))
            {
                return statistics;
            }
            //TODO: check if its working correctly and combine with RangedWeapon.cs

            Debug.LogError("STATISTICS ARE EMPTY");

            return statistics;
        }
    }
}
