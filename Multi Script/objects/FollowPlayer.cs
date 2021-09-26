using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FollowPlayer : MonoBehaviourPun
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        if (!photonView.IsMine)
            return;
        transform.position = target.position + offset;
    }
}
