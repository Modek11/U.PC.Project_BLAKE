using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interacter);
    bool CanInteract();
    GameObject GetGameObject();
    Vector3 GetPositionForUI();
}
