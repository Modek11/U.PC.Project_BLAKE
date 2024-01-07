using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDoor : Door, IInteractable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RoomConnector connector;
    [SerializeField]
    private bool interactable = false;
    private bool open = false;

    private void Awake()
    {
        if(animator == null)
        animator = GetComponent<Animator>();
        LockDoor();
    }
    public override void UnlockDoor()
    {
        interactable = true;
    }

    public override void LockDoor()
    {
        interactable = false;
        CloseDoor();
    }


    public void Interact(GameObject interacter)
    {
        if (!CanInteract()) return;

        if(open)
        {
            CloseDoor();
            connector.CloseDoor();
        } else
        {
            OpenDoor();
            connector.OpenDoor();

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
    public override void OpenDoor()
    {
        animator.SetBool("closed", false);
        //if (open) return;
        open = true;

    }

    public override void CloseDoor()
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
