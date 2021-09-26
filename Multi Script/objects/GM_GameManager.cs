using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GM_GameManager : MonoBehaviourPunCallbacks
{

    public static GM_GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GM_GameManager>();
            return instance;
        }
    }

    private static GM_GameManager instance;

    public Transform[] spawnPositions;
    public GameObject playerPrefab;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
    }
}
