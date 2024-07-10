using _Project.Scripts;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

public class NextLevelInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform UIHolderTransform;
    public bool CanInteract()
    {
        return true;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetPositionForUI()
    {
        if (UIHolderTransform == null) return transform.position;
        return UIHolderTransform.position;
    }

    public void Interact(GameObject interacter)
    {
        ReferenceManager.LevelHandler.GoToNextLevel();
    }
}
