using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractables : MonoBehaviour
{
    //List of Interactable objects nearby
    [SerializeField]
    private List<IInteractable> interactables = new List<IInteractable>();

    //Object of UI to show above
    [SerializeField]
    private GameObject interactUI;

    private void Start()
    {
        ReferenceManager.PlayerInputController.interactEvent += Interact;
    }

    /// <summary>
    /// Add Interactable to list
    /// </summary>
    /// <param name="interactable"></param>
    public void AddInteractable(IInteractable interactable)
    {
        interactables.Add(interactable);
    }

    /// <summary>
    /// Remove Interactable from list
    /// </summary>
    /// <param name="interactable"></param>
    public void RemoveInteractable(IInteractable interactable)
    {
        interactables.Remove(interactable);
    }

    private void Update()
    {
        SetUI();
    }

    private IInteractable GetClosestInteractable()
    {
        if (interactables.Count > 0)
        {
            IInteractable closest = null;
            float closestDistance = float.MaxValue;

            List<IInteractable> invalidInteractables = new List<IInteractable>();

            foreach (IInteractable interactable in interactables)
            {
                if (!interactable.CanInteract()) continue;
                if (interactable.GetGameObject() == null) { invalidInteractables.Add(interactable); continue; }

                if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < closestDistance)
                {
                    closest = interactable;
                    closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                }
            }

            for(int i = 0; i < invalidInteractables.Count; i++)
            {
                interactables.Remove(invalidInteractables[i]);
            }

            return closest;
        }
        return null;
    }

    private void Interact()
    {
        IInteractable closest = GetClosestInteractable();
        if (closest == null) return;

        closest.Interact(gameObject);
    }

    //Set UI Element above Interactable
    private void SetUI()
    {
        if (interactUI == null) return;

        IInteractable closest = GetClosestInteractable();
        if (closest == null)
        {
            interactUI.SetActive(false);
            return;
        }

        interactUI.SetActive(true);
        interactUI.transform.position = closest.GetPositionForUI();
        interactUI.transform.LookAt(Camera.main.transform);
    }

    public void SetInteractUIReference(GameObject interactUIReference)
    {
        interactUI = interactUIReference;
    }
}
