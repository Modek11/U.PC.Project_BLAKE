using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInteract : Interactable
{
    //Debug Text to show
    [SerializeField]
    private string DebugText;

    public override void Interact()
    {
        Debug.Log(DebugText);
    }
}
