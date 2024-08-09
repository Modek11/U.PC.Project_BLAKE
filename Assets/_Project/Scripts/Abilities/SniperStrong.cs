using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.Weapons;
using GameFramework.Abilities;

namespace _Project.Scripts.Abilities
{
    public class SniperStrong : WeaponAbility
    {
        public override bool CanActivateAbility()
        {
            if (weaponSource is null)
            {
                weaponSource = SourceObject as Weapon;
                if (weaponSource is null)
                {
                    return false; 
                }
            }
            return true;
        }
        
        protected override void AbilitySkill()
        {
            ReferenceManager.MainVirtualCameraController.ZoomOutAndResetWithDelay();
            
            weaponSource.WeaponsManager.OnWeaponChangedEvent -= ResetZoomAndUnsubscribe;
            weaponSource.WeaponsManager.OnWeaponChangedEvent += ResetZoomAndUnsubscribe;
        }

        private void ResetZoomAndUnsubscribe(Weapon weapon)
        {
            ReferenceManager.MainVirtualCameraController.ResetZoom();
            weaponSource.WeaponsManager.OnWeaponChangedEvent -= ResetZoomAndUnsubscribe;
        }
    }
}
