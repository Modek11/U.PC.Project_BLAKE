using GameFramework.Abilities;
using System.Collections;
using UnityEngine;

public class A_MeeleAttack : Ability
{
    private MeleeWeapon weaponSource;

    public override bool CanActivateAbility()
    {
        if (weaponSource == null)
        {
            weaponSource = SourceObject as MeleeWeapon;
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
