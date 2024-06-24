using UnityEngine;

public class MechanicalDoor : Door, IInteractable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RoomConnector connector;
    [SerializeField]
    private Transform uiHolder;
    [SerializeField]
    private bool interactable = false;
    [SerializeField]
    private bool open = false;

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        LockDoor();
    }

    private void Start()
    {
        CloseDoor();
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
            connector.GetConnector().CloseDoor();
        } 
        else
        {
            OpenDoor();
            connector.GetConnector().OpenDoor();
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
        if (IsAnyAnimationPlaying())
        {
            return;
        }
        
        animator.SetBool("closed", false);
        open = true;
    }

    public override void CloseDoor()
    {
        if (IsAnyAnimationPlaying())
        {
            return;
        }
        
        animator.SetBool("closed", true);
        open = false;
    }

    public Vector3 GetPositionForUI()
    {
        return uiHolder.position;
    }
    
    private bool IsAnyAnimationPlaying()
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        return currentState.normalizedTime < 1 && animator.GetCurrentAnimatorClipInfo(0).Length > 0;
    }
}
