using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.EditorTools;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    private Room parent;
    private RoomManager roomManager;
    private bool playerInside = false;

    private void Awake()
    {
        parent = GetRoomFromParents(gameObject);
        parent.AddTrigger(this);
    }

    private Room GetRoomFromParents(GameObject p)
    {
        Room parent = GetComponentInParent<Room>();
        if (parent == null)
        {
            if (transform.parent == null) return null;
            parent = GetRoomFromParents(p.transform.parent.gameObject);
        }
        return parent;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (roomManager == null) roomManager = parent.GetRoomManager();
        if (other.CompareTag("Player"))
        {
            parent.SetPlayer(other.gameObject);
            parent.EnterRoom();
            playerInside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            parent.ExitRoom();
        }
    }

    public bool IsPlayerInside()
    {
        return playerInside;
    }

    public void Reset()
    {
        playerInside = false;
    }
}
