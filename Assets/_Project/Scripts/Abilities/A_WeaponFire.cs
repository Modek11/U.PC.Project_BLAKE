using GameFramework.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_WeaponFire : Ability
{
    private IWeapon weaponSource;
    private float lastFireTime;

    public override bool CanActivateAbility()
    {
        if(weaponSource == null)
        {
            weaponSource = SourceObject as IWeapon;
            if(weaponSource == null ) { return false; }
        }
        return base.CanActivateAbility();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        

        EndAbility();
    }
}
