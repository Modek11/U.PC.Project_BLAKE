using _Project.Scripts.GlobalHandlers;
using UnityEngine;

namespace _Project.Scripts.Weapon.Upgrades
{
    public class WeaponUpgradesInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Transform UIHolderTransform;
        
        public bool CanInteract()
        {
            return true;
        }
        
        public void Interact(GameObject interacter)
        {
            GameHandler.Instance.OpenWeaponUpgradesCanvas();
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
