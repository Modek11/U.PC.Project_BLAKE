using GameFramework.Abilities;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon definition", menuName = "Project BLAKE/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public GameObject WeaponPrefab;

    public string WeaponName;

    public AbilityDefinition[] AbilitiesToGrant;

    [Range(-0.01f, 1f)]
    public float DropRate = 0.6f;

    public string AttachSocketName;

    public Vector3 LocationOffset;
    
    public Quaternion Rotation;

    public Vector3 Scale = Vector3.one;

    public GameObject WeaponGFX;

    public Vector3 PickupLocationOffset = Vector3.zero;

    public Quaternion PickupRotation;

    private void OnValidate()
    {
        if (WeaponPrefab != null)
        {
            if(WeaponPrefab.TryGetComponent<Weapon>(out var weapon))
            {
                weapon.WeaponDefinition = this;
            }
        }
    }
}
