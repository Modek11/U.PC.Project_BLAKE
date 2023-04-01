using System.Xml.Serialization;
using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private WeaponDefinition weaponToPickup;
    [SerializeField] private Mesh pickupMesh;

    private void Awake()
    {
        if(pickupMesh != null)
        {
            ChangeMesh(pickupMesh);
        }
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
                ChangeMesh(weaponDefinition.pickupMesh);
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

    private void ChangeMesh(Mesh newMesh)
    {
        pickupMesh = newMesh;
    }
}
