using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.GlobalHandlers;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractables : MonoBehaviour
{
    [SerializeField]
    private float interactRadius = 10f;
    [SerializeField]
    private LayerMask collectMask;
    [SerializeField]
    private LayerMask raycastMask;
    //List of Interactable objects nearby
    [SerializeField]
    private List<IInteractable> interactables = new List<IInteractable>();
    [SerializeField]
    private Transform playerHead;

    //Object of UI to show above
    [SerializeField]
    private GameObject interactUI;

    private void Start()
    {
        ReferenceManager.PlayerInputController.interactEvent += Interact;
        InvokeRepeating("CheckForInteractables", 0f, 0.2f);
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

    private void CheckForInteractables()
    {
        interactables.Clear();
        Collider[] cols = Physics.OverlapSphere(transform.position, interactRadius, collectMask, QueryTriggerInteraction.Collide);
        foreach(var col in cols)
        {
            if(col.GetComponent<IInteractable>() != null)
            {
                interactables.Add(col.GetComponent<IInteractable>());
            }
        }
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

                /*if (Physics.Raycast(new Ray(playerHead.position, interactable.GetGameObject().transform.position - playerHead.position), out RaycastHit hit))
                {
                    if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < closestDistance)
                    {
                        if (hit.transform.gameObject == interactable.GetGameObject())
                        {
                            closest = interactable;
                        }
                        else if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < 0.7f)
                        {
                            closest = interactable;
                            closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                        }
                    }
                    else if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < 0.5f)
                    {
                        closest = interactable;
                        closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                    }
                }*/

                if(Physics.Raycast(new Ray(playerHead.position, interactable.GetGameObject().transform.position - playerHead.position), out RaycastHit hit, interactRadius, raycastMask))
                {
                    if (hit.transform.gameObject == interactable.GetGameObject())
                    {
                        closest = interactable;
                    }
                    else if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < .5f)
                    {
                        closest = interactable;
                        closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                    }
                }
                else if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < .5f)
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
