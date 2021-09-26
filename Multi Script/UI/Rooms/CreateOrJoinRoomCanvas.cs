using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoom createRoom;
    [SerializeField]
    private RoomListingsMenu roomListingsMenu;

    private RoomsCanvas roomsCanvas;
    public void FirstInitialize(RoomsCanvas canvases)
    {
        roomsCanvas = canvases;
        createRoom.FirstInitialize(canvases);
        roomListingsMenu.FirstInitialize(canvases);
    }
}
