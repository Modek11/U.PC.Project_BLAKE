using System.Xml.Serialization;
using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private WeaponDefinition weaponToPickup;
    [SerializeField] private GameObject pickupGameObject;
    [SerializeField] private float rotateForce = 12f;

    private GameObject weaponGFX;

    private void Awake()
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
            weaponDefinition = weaponsManager.GetWeaponDefinition(weaponsManager.ActiveWeaponIndex);
        }

        if (weaponsManager.ChangeItem(weaponToPickup, index))
        {
            if(weaponDefinition != null)
            {
                weaponToPickup = weaponDefinition;
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
}
