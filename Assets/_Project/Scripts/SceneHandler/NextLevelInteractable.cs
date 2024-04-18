using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
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
