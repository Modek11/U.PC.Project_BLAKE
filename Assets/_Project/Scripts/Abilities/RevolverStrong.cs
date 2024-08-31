using System;
using _Project.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using GameFramework.Abilities;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class RevolverStrong : WeaponAbility
    {
        public override bool CanActivateAbility()
        {
            return base.CanActivateAbility();
        }
        
        protected override void AbilitySkill()
        {
            if (weaponSource is RangedWeapon rangedWeapon)
            {
                _ = MultiShoot(rangedWeapon);
            }
            else
            {
                Debug.LogError("Wrong weapon type is attached!");
            }
        }

        private async UniTaskVoid MultiShoot(RangedWeapon rangedWeapon)
        {
            var statistics = rangedWeapon.CurrentRangedWeaponStatistics;
            statistics.SpreadType = SpreadType.NoSpreadThenStatic;
            statistics.Spread = 45f;
            statistics.SpreadResetThreshold = 5f;
            rangedWeapon.ApplyRangedWeaponStatistics(statistics);

            var bulletsLeft = rangedWeapon.BulletsLeft;
            for (var i = 0; i <= bulletsLeft; i++)
            {
                rangedWeapon.PrimaryAttack();
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }

            //DropWeapon
            rangedWeapon.CanPrimaryAttack();
            
            rangedWeapon.RestoreRangedWeaponStatistics();
        }
    }
}
