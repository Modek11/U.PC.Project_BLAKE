using _Project.Scripts;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "AgilityThreePerk", menuName = "Abilities/Perks/Agility Three")]

public class AgilityThreePerk : PerkScriptableObject
{
    [SerializeField]
    private PlayerMovement playerMovement;
    public int dashesToAdd = 1;
    public float enemyBoostSpeed = 2;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerMovement = perkManager.GetComponent<PlayerMovement>();
        playerMovement?.AddDashes(dashesToAdd);
        ReferenceManager.RoomManager.onRoomEnter += SubscribeToRoom;
        ReferenceManager.RoomManager.onRoomLeave += UnsubscribeToRoom;
    }

    private void SubscribeToRoom(Room room)
    {
        room.enemySpawned += BoostEnemy;
    }

    private void UnsubscribeToRoom(Room room)
    {
        room.enemySpawned -= BoostEnemy;
    }

    private void BoostEnemy(EnemyCharacter enemy)
    {
        enemy.AddAdditionalSpeed(enemyBoostSpeed);
    }

    public override void OnRemove()
    {
        playerMovement?.RemoveDashes(dashesToAdd);
    }
}
