using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Connected To Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try Reconnecting";
        // 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            // 랜덤 룸에 자동 접속
            connectionInfoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline : Connection Disabled - Try Reconnecting...";
            // 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }

    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
;       connectionInfoText.text = "There is no empty room. Creating new room...";

        // 여기 null은 room name인데 나중에 바꿔도 될듯.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room";
        PhotonNetwork.LoadLevel("Main");
    }
}
