using _Project.Scripts.GlobalHandlers;
using GameFramework.Abilities;

namespace _Project.Scripts.Abilities
{
    public class SniperStrong : WeaponAbility
    {
        public override bool CanActivateAbility()
        {
            return true;
        }
        
        protected override void AbilitySkill()
        {
            ReferenceManager.MainVirtualCameraController.ChangeZoom();
            
        }
    }
}
