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

        public void AddWeaponUpgrade(string weaponName, RangedWeaponStatistics weaponStatistics)
        {
            var passedWeaponName = weaponName.ToLower().Replace(" ", "");
            foreach (var weaponUpgrade in rangedWeaponsUpgrades)
            {
                var weaponDefinitionName = weaponUpgrade.Key.WeaponName.ToLower().Replace(" ", "");
                if (passedWeaponName == weaponDefinitionName)
                {
                    var upgradedValues = weaponUpgrade.Value + weaponStatistics;
                    rangedWeaponsUpgrades[weaponUpgrade.Key] = upgradedValues;
                    return;
                }
            }
        }
        
        public void AddWeaponUpgrade(string weaponName, MeleeWeaponStatistics weaponStatistics)
        {
            var passedWeaponName = weaponName.ToLower().Replace(" ", "");
            foreach (var weaponUpgrade in meleeWeaponsUpgrades)
            {
                var weaponDefinitionName = weaponUpgrade.Key.WeaponName.ToLower().Replace(" ", "");
                if (passedWeaponName == weaponDefinitionName)
                {
                    var upgradedValues = weaponUpgrade.Value + weaponStatistics;
                    meleeWeaponsUpgrades[weaponUpgrade.Key] = upgradedValues;
                    return;
                }
            }
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
