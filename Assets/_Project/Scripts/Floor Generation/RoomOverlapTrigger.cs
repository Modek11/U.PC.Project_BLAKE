using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOverlapTrigger : MonoBehaviour
{
    private Room parent;
    private bool playerInside = false;

    // Start is called before the first frame update
    void Awake()
    {
        parent = GetComponentInParent<Room>();
        parent.AddFogTrigger(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.DisableFog();
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        playerInside = false;
        if (other.CompareTag("Player"))
        parent.EnableFog();
    }
    public bool IsPlayerInside()
    {
        return playerInside;
    }

    public void Reset()
    {
        playerInside = false;
        parent.EnableFog();

    }
}
