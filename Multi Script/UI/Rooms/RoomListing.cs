using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public RoomInfo Roominfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        Roominfo = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(Roominfo.Name);
    }
}
