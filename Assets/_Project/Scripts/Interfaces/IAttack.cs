using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public bool Attack(Weapon weapon);

    public float ReturnFireRate();
}
