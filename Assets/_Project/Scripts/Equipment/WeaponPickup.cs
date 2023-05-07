using System.Xml.Serialization;
using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private WeaponDefinition weaponToPickup;
    [SerializeField] private GameObject pickupGameObject;
    [SerializeField] private float rotateForce = 12f;

    public int ammo = -1;
    private GameObject weaponGFX;

    private void Start()
    {
        weaponGFX = Instantiate(weaponToPickup.weaponGFX, pickupGameObject.transform.position, pickupGameObject.transform.rotation, pickupGameObject.transform);
    }

    private void Update()
    {
        pickupGameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotateForce);
    }

    public override void Interact(GameObject interacter)
    {
        WeaponsManager weaponsManager = interacter.GetComponent<WeaponsManager>();
        if(weaponsManager == null)
        {
            Debug.LogWarning("WeaponsManager is not valid");
            return;
        }

        int index = weaponsManager.ActiveWeaponIndex == 0 ? 1 : weaponsManager.ActiveWeaponIndex;

        WeaponDefinition weaponDefinition = null;
        if(weaponsManager.IsWeaponValid(index))
        {
            weaponDefinition = weaponsManager.GetWeaponDefinition(index);
        }

        int bulletsLeft = 0;

        IWeapon oldWeapon = weaponsManager.GetIWeapon(index);
        if (oldWeapon != null)
        {
            bulletsLeft = oldWeapon.GetGameObject().GetComponent<Weapon>().BulletsLeft;
        }

        if (weaponsManager.ChangeItem(weaponToPickup, index))
        {
            if (ammo != -1)
            {
                weaponsManager.GetIWeapon(index).SetAmmo(ammo);
            }

            weaponsManager.OnPlayerPickupWeapon();

            if (weaponDefinition != null)
            {
                weaponToPickup = weaponDefinition;
                ammo = bulletsLeft;
                ChangeVisuals(weaponDefinition);
                return;
            }

            PlayerInteractables playerInteractables = interacter.GetComponent<PlayerInteractables>();
            if (playerInteractables != null)
            {
                playerInteractables.RemoveInteractable(this);
            }
            Destroy(gameObject);
        }
    }

    private void ChangeVisuals(WeaponDefinition newWeapon)
    {
        if (weaponGFX != null)
        {
            Destroy(weaponGFX);
        }

        weaponGFX = Instantiate(newWeapon.weaponGFX, pickupGameObject.transform);
        weaponGFX.transform.localPosition = newWeapon.pickupLocationOffset;
        weaponGFX.transform.localRotation = newWeapon.pickupRotation;
    }

    public void SetWeaponDefinition(WeaponDefinition wd)
    {
        weaponToPickup = wd;
    }
}
