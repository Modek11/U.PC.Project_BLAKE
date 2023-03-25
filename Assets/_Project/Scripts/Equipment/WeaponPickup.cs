using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private WeaponDefinition weaponToPickup;

    public override void Interact(GameObject interacter)
    {
        WeaponsManager weaponsManager = interacter.GetComponent<WeaponsManager>();
        if(weaponsManager == null)
        {
            Debug.LogWarning("WeaponsManager is not valid");
            return;
        }

        if(weaponsManager.ChangeItem(weaponToPickup, weaponsManager.ActiveWeaponIndex == 0 ? 1 : weaponsManager.ActiveWeaponIndex))
        {
            PlayerInteractables playerInteractables = interacter.GetComponent<PlayerInteractables>();
            if(playerInteractables != null)
            {
                playerInteractables.RemoveInteractable(this);
            }

            Destroy(gameObject);
        }
    }
}
