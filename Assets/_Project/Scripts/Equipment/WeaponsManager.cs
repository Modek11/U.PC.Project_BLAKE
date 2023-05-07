using UnityEngine;

using System;
using System.Collections.Generic;

public class WeaponsManager : MonoBehaviour
{
    public event Action changeWeaponEvent;
    public event Action onPlayerPickupWeaponEvent;
    public event Action onSuccessfulShotEvent;

    [SerializeField] private List<GameObject> attachSockets = new List<GameObject>();

    private int capacity = 2;
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

    private int activeWeaponIndex = 0;
    public int ActiveWeaponIndex { get { return activeWeaponIndex; } }

    private ValueTuple<WeaponDefinition, IWeapon>[] weaponItems = new ValueTuple<WeaponDefinition, IWeapon>[3] { (null, null), (null, null), (null, null) };

    public WeaponDefinition defaultWeapon;

    PlayerInputController playerInputController;

    private void Awake()
    {
        if (defaultWeapon == null)
        {
            Debug.LogWarning("Default equipment item is not valid");
            return;
        }
        ChangeItem(defaultWeapon, 0);
        Equip(0);

        playerInputController = GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            playerInputController.changeWeaponEvent += Equip;
            playerInputController.shootEvent += ShootWeapon;
        }
    }

    //Use to add or replace item
    public bool ChangeItem(WeaponDefinition item, int index)
    {
        if (item == null) return false;
        if (index < 0 || index > capacity - 1) return false;

        if (IsWeaponValid(index))
        {
            Destroy(weaponItems[index].Item2.GetGameObject());
            weaponItems[index].Item2 = null;
        }

        weaponItems[index].Item1 = item;
        Equip(index);
        return true;
    }

    public void Equip(int index)
    {
        if (index < 0 || index > capacity - 1) return;
        if (weaponItems[index].Item1 == null) return;

        SetCurrentWeaponActive(false);

        activeWeaponIndex = index;

        if (!SetCurrentWeaponActive(true))
        {
            SpawnWeapon();
        }
        
        changeWeaponEvent?.Invoke();
    }

    public void ShootWeapon()
    {
        if(weaponItems[activeWeaponIndex].Item2.PrimaryAttack())
        {
            onSuccessfulShotEvent?.Invoke();
        }
    }

    public void OnPlayerPickupWeapon()
    {
        onPlayerPickupWeaponEvent?.Invoke();
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

        var weapon = Instantiate(weaponItems[activeWeaponIndex].Item1.weaponPrefab, spawnLocation, spawnRotation, transform);
        weapon.transform.localScale = weaponItems[activeWeaponIndex].Item1.scale;

        weaponItems[activeWeaponIndex].Item2 = weapon.GetComponent<IWeapon>();
    }

    private bool SetCurrentWeaponActive(bool newActive)
    {
        if (IsWeaponValid(activeWeaponIndex))
        {
            weaponItems[activeWeaponIndex].Item2.GetGameObject().SetActive(newActive);
            return true;
        }
        return false;
    }

    public bool IsWeaponValid(int index)
    {
        if (index < 0 || index > capacity - 1) return false;

        return weaponItems[index].Item2 != null;
    }

    public WeaponDefinition GetWeaponDefinition(int index)
    {
        if (index < 0 || index > capacity - 1) return null;

        return weaponItems[index].Item1;
    }
    
    public IWeapon GetIWeapon(int index)
    {
        if (index < 0 || index > capacity - 1) return null;

        return weaponItems[index].Item2;
    }
}