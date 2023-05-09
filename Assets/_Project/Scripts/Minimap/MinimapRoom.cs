using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinimapRoom : MonoBehaviour
{
    private enum RoomState
    {
        Unseen,
        NotVisited,
        Visited
    }

    [SerializeField]
    private Material minimapUnseen;
    [SerializeField]
    private Material minimapNotVisited;
    [SerializeField]
    private Material minimapVisited;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    private RoomState state = RoomState.Unseen;

    private void Start()
    {
        ChangeMaterial(minimapUnseen);
    }
    public void ShowRoom()
    {
        if(state == RoomState.Unseen)
        {
            state = RoomState.NotVisited;
            ChangeMaterial(minimapNotVisited);
        }
    }

    public void VisitRoom()
    {
        if(state == RoomState.NotVisited)
        {
            state = RoomState.Visited;
            ChangeMaterial(minimapVisited);
        }
    }

    private void ChangeMaterial(Material mat)
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = mat;
        }
    }
}
