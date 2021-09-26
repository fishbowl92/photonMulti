using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField nickname;
    [SerializeField]
    private Button connectBtn;
    [SerializeField]
    private GameObject loadingIcon;
    [SerializeField]
    private Canvas networkCanvas;
    [SerializeField]
    private Canvas roomCanvas;
    

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        networkCanvas.gameObject.SetActive(true);
        roomCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (nickname.text == "")
            connectBtn.interactable = false;
        else
            connectBtn.interactable = true;
    }
    public void Onclick_Connect()
    {
        PhotonNetwork.NickName = nickname.text;
        loadingIcon.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        networkCanvas.gameObject.SetActive(false);
        roomCanvas.gameObject.SetActive(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from a server for this reason: {cause.ToString()}");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
}
