using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDoor : Door, IInteractable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RoomConnector connector;
    private bool interactable = false;
    private bool open = false;

    private void Awake()
    {
        if(animator == null)
        animator = GetComponent<Animator>();
        Close();
    }
    public override void OpenDoor()
    {
        interactable = true;
    }

    public override void CloseDoor()
    {
        interactable = false;
        Close();
    }


    public void Interact(GameObject interacter)
    {
        if (!CanInteract()) return;

        if(open)
        {
            Close();
        } else
        {
            Open();
        }
    }

    public bool CanInteract()
    {
        return interactable;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    private void Open()
    {
        animator.SetBool("closed", false);
        //if (open) return;
        open = true;

    }

    private void Close()
    {
        animator.SetBool("closed", true);
        //if (!open) return;

        open = false;
        
    }

    public Vector3 GetPositionForUI()
    {
        return transform.position + transform.forward + Vector3.up;
    }
}
