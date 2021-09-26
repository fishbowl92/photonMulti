using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField roomName;
    [SerializeField]
    private TextMeshProUGUI statusText;
    [SerializeField]
    private Button createRoomBtn;

    private RoomsCanvas roomsCanvas;
    public void FirstInitialize(RoomsCanvas canvases)
    {
        roomsCanvas = canvases;
    }

    private void Update()
    {
        if (roomName.text == "")
            createRoomBtn.interactable = false;
        else
            createRoomBtn.interactable = true;
    }

    public void OnClick_CreateRoom()
    {
        // CreateRoom
        if (!PhotonNetwork.IsConnected)
        {
            statusText.text = "Disconnected from the server!";
            return;
        }
        statusText.text = "Creating room...";
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
        statusText.text = "Created room successfully";
        roomsCanvas.CurrentRoomCanvas.Show();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "Room creation failed!";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");
        statusText.text = "Joined room successfully";
    }


}
