using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Abilities;
using static UnityEditor.Progress;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> attachSockets = new List<GameObject>();

    public event Action changeWeaponEvent;
    public event Action onPlayerPickupWeaponEvent;
    public event Action onSuccessfulShotEvent;

    private int activeWeaponIndex = 0;
    public int ActiveWeaponIndex { get => activeWeaponIndex; }

    private List<IWeapon> weapons = new List<IWeapon>() { null, null };
    public List<IWeapon> Weapons { get => weapons; }

    public WeaponDefinition defaultWeapon;
    private AbilityManager abilityManager;

    private void Awake()
    {
        if (defaultWeapon == null)
        {
            Debug.LogWarning("Default equipment item is not valid");
            return;
        }

        abilityManager = GetComponent<AbilityManager>();

        Equip(defaultWeapon, 0);
        
        if (ReferenceManager.PlayerInputController != null)
        {
            ReferenceManager.PlayerInputController.changeWeaponEvent += SetActiveIndex;
            ReferenceManager.PlayerInputController.shootEvent += ShootWeapon;
        }

        WeaponsMagazine.Init();
    }

    public void Equip(WeaponDefinition weaponDefinition, int index)
    {
        if (weaponDefinition == null) return;
        if (index < 0 || index >= weapons.Count) return;

        Unequip(index);
        SpawnWeapon(weaponDefinition, index);

        if(index == activeWeaponIndex)
        {
            GiveWeaponAbilities();
            changeWeaponEvent?.Invoke();
        }
    }

    public void Unequip(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        if (weapons[index] != null)
        {
            if (index == activeWeaponIndex)
            {
                RemoveWeaponAbilities();
            }

            Destroy(weapons[index].GetWeapon());
            weapons[index] = null;
        }
    }

    public void SetActiveIndex(int index)
    {
        if (index < 0 || index >= weapons.Count) return;
        if (weapons[index] == null) return;
        if (index == activeWeaponIndex) return;

        if (weapons[activeWeaponIndex] != null)
        {
            weapons[activeWeaponIndex].GetWeapon().SetActive(false);
            RemoveWeaponAbilities();
        }

        if (weapons[index] != null)
        {
            activeWeaponIndex = index;
            weapons[activeWeaponIndex].GetWeapon().SetActive(true);
            GiveWeaponAbilities();

            changeWeaponEvent?.Invoke();
        }
    }

    public void ShootWeapon()
    {
        if(weapons[activeWeaponIndex].PrimaryAttack())
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

        for(int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == null)
            {
                freeIndex = i;
                break;
            }
        }
        return freeIndex;
    }

    private void SpawnWeapon(WeaponDefinition weaponDefinition, int index)
    {
        Transform socketTransform = transform;
        foreach(var socket in attachSockets)
        {
            if(socket.name == weaponDefinition.attachSocketName)
            {
                socketTransform = socket.transform;
                break;
            }
        }

        Vector3 spawnLocation = socketTransform.position + weaponDefinition.locationOffset;
        Quaternion spawnRotation = weaponDefinition.rotation;

        var weapon = Instantiate(weaponDefinition.weaponPrefab, spawnLocation, spawnRotation, transform);
        weapon.transform.localScale = weaponDefinition.scale;
        weapon.SetActive(index == activeWeaponIndex);

        if (weapon.TryGetComponent(out Weapon weaponComp))
        {
            weaponComp.SetOwner(gameObject);
        }

        weapons[index] = weapon.GetComponent<IWeapon>();
    }

    private void GiveWeaponAbilities()
    {
        foreach (var ability in weapons[activeWeaponIndex].GetWeaponDefinition().AbilitiesToGrant)
        {
            abilityManager.GiveAbility(ability, weapons[activeWeaponIndex]);
        }
    }

    private void RemoveWeaponAbilities()
    {
        foreach (var ability in weapons[activeWeaponIndex].GetWeaponDefinition().AbilitiesToGrant)
        {
            abilityManager.RemoveAbility(ability);
        }
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