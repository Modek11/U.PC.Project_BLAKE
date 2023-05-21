using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

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
                Destroy(weaponItems[2].Item2.GetWeapon()); 
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

        WeaponsMagazine.Init();
    }

    //Use to add or replace item
    public bool ChangeItem(WeaponDefinition item, int index)
    {
        if (item == null) return false;
        if (index < 0 || index > capacity - 1) return false;

        if (IsWeaponValid(index))
        {
            Destroy(weaponItems[index].Item2.GetWeapon());
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

    public void DestroyWeapon(int index)
    {
        if (index < 0 || index > capacity - 1) return;

        if (IsWeaponValid(index))
        {
            Destroy(weaponItems[index].Item2.GetWeapon());
            weaponItems[index].Item2 = null;
            weaponItems[index].Item1 = null;

            Equip(0);
        }
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

        if(weapon.TryGetComponent(out Weapon weaponComp))
        {
            weaponComp.SetOwner(gameObject);
        }

        weaponItems[activeWeaponIndex].Item2 = weapon.GetComponent<IWeapon>();
    }

    private bool SetCurrentWeaponActive(bool newActive)
    {
        if (IsWeaponValid(activeWeaponIndex))
        {
            weaponItems[activeWeaponIndex].Item2.GetWeapon().SetActive(newActive);
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

public static class WeaponsMagazine //XD
{
    static List<WeaponDefinition> weapons;
    static bool Inited = false;

    public static void Init()
    {
        if (Inited) return;

        weapons = Resources.LoadAll<WeaponDefinition>("Weapons").ToList();
        
        int katanaIndex = -1;
        int batIndex = -1;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].name == "Katana")
            {
                katanaIndex = i;
            }
            if (weapons[i].name == "Bat")
            {
                batIndex = i;
            }
        }
        if (katanaIndex == -1 || batIndex == -1) return;

        weapons.RemoveAt(katanaIndex);
        weapons.RemoveAt(batIndex);

        string weaponsText = weapons[0].name;
        for (int i = 1; i < weapons.Count; i++)
        {
            weaponsText += ", " + weapons[i].name;
        }
        Debug.Log("Weapons count: " + weapons.Count + " - " + weaponsText);

        Inited = true;
    }

    public static WeaponDefinition GetRandomWeapon()
    {
        WeaponDefinition randomWeapon = weapons[UnityEngine.Random.Range(0, weapons.Count)];
        Debug.Log("Random weapom: " + randomWeapon.name);
        return randomWeapon;
    }

    public static int GetRandomWeaponAmmo(WeaponDefinition weapon)
    {
        int randomWeaponAmmo = UnityEngine.Random.Range(weapon.magazineSize / 2, weapon.magazineSize + 1);
        Debug.Log("Random ammo for " + weapon.name + " weapon: " + randomWeaponAmmo);
        return randomWeaponAmmo;
    }
}