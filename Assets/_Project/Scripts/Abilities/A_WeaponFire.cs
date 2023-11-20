using GameFramework.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_WeaponFire : Ability
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();

        Debug.Log("==============Activate==============");

        EndAbility();
    }
}
