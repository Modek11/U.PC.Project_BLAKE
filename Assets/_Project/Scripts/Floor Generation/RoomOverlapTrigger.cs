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
        parent = GetRoomFromParents(gameObject);
        parent.AddFogTrigger(this);

    }

    private Room GetRoomFromParents(GameObject p)
    {
        Room parent = GetComponentInParent<Room>();
        if(parent == null)
        {
            if (transform.parent == null) return null;
            parent = GetRoomFromParents(p.transform.parent.gameObject);
        }
        return parent;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.DisableFog();
            playerInside = true;
        }
    }
    private void OnTriggerStay(Collider other)
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
