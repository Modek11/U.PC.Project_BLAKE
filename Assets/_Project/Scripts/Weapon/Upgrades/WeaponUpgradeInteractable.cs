using _Project.Scripts.GlobalHandlers;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class WeaponUpgradeInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Transform UIHolderTransform;
        
        public bool CanInteract()
        {
            return ReferenceManager.WeaponUpgradeManager.IsUpgradeAvailable;
        }
        
        public void Interact(GameObject interacter)
        {
            ReferenceManager.WeaponUpgradeManager.TryShowWeaponUpgrades();
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Vector3 GetPositionForUI()
        {
            return !UIHolderTransform ? transform.position : UIHolderTransform.position;
        }
    }
}
