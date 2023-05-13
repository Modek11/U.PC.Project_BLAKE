using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDoor : Door
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void OpenDoor()
    {
        animator.SetBool("closed", false);
    }

    public override void CloseDoor()
    {
        animator.SetBool("closed", true);
    }
}
