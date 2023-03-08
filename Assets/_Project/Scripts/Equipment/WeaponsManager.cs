using UnityEngine;

using System;
using System.Collections.Generic;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> attachSockets;

    private int capacity;
    public int Capacity
    {
        get { return capacity; }
        set 
        { 
            capacity = value;

            if (activeWeaponIndex == 2)
            {
                Equip(1);
            }
            else if (weaponItems[2].Item2 != null)
            {
                Destroy(weaponItems[2].Item2.GetGameObject()); 
            }
            weaponItems[2] = (null, null);
        }
    }

    private int activeWeaponIndex;
    public int ActiveWeaponIndex { get { return activeWeaponIndex; } }

    private ValueTuple<EquipmentItemDefinition, IWeapon>[] weaponItems;

    public EquipmentItemDefinition defaultWeapon;

    WeaponsManager()
    {
        attachSockets = new List<GameObject>();
        capacity = 2;
        activeWeaponIndex = 0;
        weaponItems = new ValueTuple<EquipmentItemDefinition, IWeapon>[3] { (null, null), (null, null), (null, null) };
    }

    private void Awake()
    {
        if (defaultWeapon == null)
        {
            Debug.LogWarning("Default equipment item is not valid");
            return;
        }
        ChangeItem(defaultWeapon, 0);
    }

    //Use to add or replace item
    public bool ChangeItem(EquipmentItemDefinition item, int index)
    {
        if (item == null) return false;
        if (index < 0 || index > capacity - 1) return false;

        weaponItems[index].Item1 = item;

        if(activeWeaponIndex == index)
        {
            Equip(index);
        }

        return true;
    }

    public void Equip(int index)
    {
        if (index < 0 || index > capacity - 1) return;
        if (weaponItems[index].Item1 == null) return;

        if (weaponItems[activeWeaponIndex].Item2 != null)
        {
            Destroy(weaponItems[activeWeaponIndex].Item2.GetGameObject());
        }
        activeWeaponIndex = index;
        SpawnWeapon();
    }

    public int GetFreeIndex()
    {
        int freeIndex = -1;

        for(int i = 0; i < capacity; i++)
        {
            if (weaponItems[i].Item1 == null)
            {
                freeIndex = i;
                break;
            }
        }
        return freeIndex;
    }

    private void SpawnWeapon()
    {
        Transform socketTransform = transform;
        foreach(var socket in attachSockets)
        {
            if(socket.name == weaponItems[activeWeaponIndex].Item1.attachSocketName)
            {
                socketTransform = socket.transform;
                break;
            }
        }

        Vector3 spawnLocation = socketTransform.position + weaponItems[activeWeaponIndex].Item1.locationOffset;
        Quaternion spawnRotation = weaponItems[activeWeaponIndex].Item1.rotation;

        var weapon = Instantiate(weaponItems[activeWeaponIndex].Item1.equipmentPrefab, spawnLocation, spawnRotation, transform);
        weapon.transform.localScale = weaponItems[activeWeaponIndex].Item1.scale;

        weaponItems[activeWeaponIndex].Item2 = weapon.GetComponent<IWeapon>();
    }
}