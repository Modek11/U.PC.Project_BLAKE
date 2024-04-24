using GameFramework.Abilities;
using _Project.Scripts.Weapon;

public class A_WeaponAttack : Ability
{
    private Weapon weaponSource;

    public override bool CanActivateAbility()
    {
        if (weaponSource == null)
        {
            weaponSource = SourceObject as Weapon;
            if (weaponSource == null) { return false; }
        }
        return weaponSource.CanPrimaryAttack() && base.CanActivateAbility();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        weaponSource.PrimaryAttack();

        EndAbility();
    }
}
