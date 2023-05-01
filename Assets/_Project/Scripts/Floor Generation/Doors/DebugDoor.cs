using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDoor : Door
{
    [SerializeField]
    private Material closedMat;
    [SerializeField]
    private Material openMat;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        OpenDoor();
    }
    public override void OpenDoor()
    {
        if(meshRenderer != null)
        meshRenderer.material = openMat;
        GetComponent<BoxCollider>().enabled = false;
    }

    public override void CloseDoor()
    {
        if(meshRenderer != null)
        meshRenderer.material = closedMat;
        GetComponent<BoxCollider>().enabled = true;
    }
}
