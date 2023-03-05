using System.Collections;
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
    
    private PlayerInputSystem inputSystem;

    private void Awake()
    {
        //TO DO: Change it to new implementation of InputSystem
        inputSystem = new PlayerInputSystem();
        inputSystem.Gameplay.Interact.performed += ctx => Interact();
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
            foreach (IInteractable interactable in interactables)
            {
                if (!interactable.CanInteract()) continue;
                if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < closestDistance)
                {
                    closest = interactable;
                    closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                }
            }
            return closest;
        }
        return null;
    }

    private void Interact()
    {
        IInteractable closest = GetClosestInteractable();
        if (closest == null) return;

        closest.Interact();
    }

    //Set UI Element above Interactable
    private void SetUI()
    {
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

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
