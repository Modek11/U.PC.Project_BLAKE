using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Room parent;
    private RoomManager roomManager;
    private bool playerInside = false;
    private void Awake()
    {
        parent = GetComponentInParent<Room>();
        parent.AddTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (roomManager == null) roomManager = parent.GetRoomManager();
        if (other.CompareTag("Player"))
        {
            parent.EnterRoom();
            parent.SetPlayer(other.gameObject);
            playerInside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (roomManager.GetActiveRoom() != this)
            {
                parent.ExitRoom();
            }
            if (roomManager.GetActiveRoom() == null)
            {
                parent.EnterRoom();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;
        if (other.CompareTag("Player"))
        {
            parent.ExitRoom();
        }
    }

    public bool IsPlayerInside()
    {
        return playerInside;
    }
}
