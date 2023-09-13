using UnityEngine;

public class RoomsDoneCounter : MonoBehaviour
{
    private FloorGenerator floorGenerator;
    public int RoomsInitialized { get; private set; }
    public int RoomsBeaten { get; private set; }

    private void Awake()
    {
        floorGenerator = GetComponent<FloorGenerator>();
    }

    private void Start()
    {
        RoomsInitialized = floorGenerator.GetIntRoomsInitialized();
    }

    public void AddBeatenRoom()
    {
        RoomsBeaten++;
        CheckRoomsCounter();
    }

    private void CheckRoomsCounter()
    {
        if (RoomsBeaten >= RoomsInitialized)
        {
            GameHandler.Instance.PlayerWin();
        }
    }

    private void ResetValues()
    {
        RoomsBeaten = 0;
        RoomsInitialized = 0;
    }
    private void OnDestroy()
    {
        ResetValues();
    }

}
