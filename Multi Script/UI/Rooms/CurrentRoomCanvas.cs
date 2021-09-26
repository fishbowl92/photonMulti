using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingMenu playerListingMenu;
    [SerializeField]
    private LeaveRoomMenu leaveRoomMenu;

    private RoomsCanvas roomsCanvas;
    public void FirstInitialize(RoomsCanvas canvases)
    {
        roomsCanvas = canvases;
        playerListingMenu.FirstInitialize(canvases);
        leaveRoomMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
