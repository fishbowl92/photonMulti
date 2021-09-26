using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private RoomListing roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();
    private RoomsCanvas roomsCanvas;

    public void FirstInitialize(RoomsCanvas canvases)
    {
        roomsCanvas = canvases;
    }

    public override void OnJoinedRoom()
    {
        roomsCanvas.CurrentRoomCanvas.Show();
        content.DestoryChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            // Remove from room list
            if(info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.Roominfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Added to room list
            else
            {
                int index = _listings.FindIndex(x => x.Roominfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(roomListing, content);
                    if (listing != null)
                        listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
                else
                {
                    // Modify listing here...
                    // _listings[index].dowhatever...
                }
            }

        }
    }
}
