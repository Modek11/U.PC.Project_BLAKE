using GameFramework.Abilities;

namespace _Project.Scripts.Abilities
{
    public class BasicAttack : WeaponAbility
    {
        public override bool CanActivateAbility()
        {
            return base.CanActivateAbility();
        }
        
        protected override void AbilitySkill()
        {
            weaponSource.PrimaryAttack();
        }
    }
}
