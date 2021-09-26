using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomsCanvas roomsCanvas;
    public void FirstInitialize(RoomsCanvas canvases)
    {
        roomsCanvas = canvases;
    }
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        roomsCanvas.CurrentRoomCanvas.Hide();
    }


}
