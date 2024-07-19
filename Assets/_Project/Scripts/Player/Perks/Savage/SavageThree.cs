using _Project.Scripts.Weapon;
using _Project.Scripts;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

[CreateAssetMenu(fileName = "SavageThreePerk", menuName = "Abilities/Perks/Savage Three")]

public class SavageThree : PerkScriptableObject
{
    private WeaponsManager weaponsManager;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        weaponsManager = ReferenceManager.BlakeHeroCharacter.GetComponent<WeaponsManager>();
        weaponsManager.AddThirdWeaponSlot();
        ReferenceManager.RoomManager.onRoomEnter += SubscribeToRoom;
        ReferenceManager.RoomManager.onRoomLeave += UnsubscribeToRoom;
        ReferenceManager.Instance.OnFloorLoad += Refresh;

    }

    private void Refresh()
    {
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
        enemy.savageThreeActivated = true;
    }

    public override void OnRemove()
    {
        weaponsManager.RemoveThirdWeaponSlot();
        ReferenceManager.Instance.OnFloorLoad -= Refresh;

    }
}
