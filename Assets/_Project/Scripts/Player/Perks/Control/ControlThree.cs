using _Project.Scripts.GlobalHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlThreePerk", menuName = "Abilities/Perks/Control Three")]

public class ControlThree : PerkScriptableObject
{
    PlayerPerkManager player;
    public override void OnPickup(PlayerPerkManager player)
    {
        this.player = player;
        foreach(Room room in ReferenceManager.RoomManager.GetAllRooms())
        {
            room.ActivateAllBlockers();
        }
        ReferenceManager.Instance.OnFloorLoad += Refresh;
    }
    private void Refresh()
    {
        ReferenceManager.RoomManager.isControlOneActivated = true;
        foreach (Room room in ReferenceManager.RoomManager.GetAllRooms())
        {
            room.ActivateAllBlockers();
        }
    }


    public override void OnRemove()
    {
    }
}
