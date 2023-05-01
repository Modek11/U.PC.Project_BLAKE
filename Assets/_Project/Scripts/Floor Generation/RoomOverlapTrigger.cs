using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOverlapTrigger : MonoBehaviour
{
    private Room parent;
    // Start is called before the first frame update
    void Awake()
    {
        parent = GetComponentInParent<Room>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        parent.DisableFog();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        parent.EnableFog();
    }
}
