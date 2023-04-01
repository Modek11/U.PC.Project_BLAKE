using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    //Position where UI Element should appear
    [SerializeField]
    protected Transform uiHolder;

    //Can player interact with this object
    [SerializeField]
    protected bool interactable = true;

    /// <summary>
    /// Interact with object
    /// </summary>
    public abstract void Interact(GameObject interacter);

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInteractables>() != null)
        {
            other.GetComponent<PlayerInteractables>().AddInteractable(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInteractables>() != null)
        {
            other.GetComponent<PlayerInteractables>().RemoveInteractable(this);
        }
    }


    /// <returns>If object is interactable</returns>
    public bool CanInteract()
    {
        return interactable;
    }


    /// <returns>This gameObject</returns>
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    /// <returns>Position where UI should appear</returns>
    public Vector3 GetPositionForUI()
    {
        return uiHolder.position;
    }
}
