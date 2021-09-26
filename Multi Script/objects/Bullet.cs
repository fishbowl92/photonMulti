using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPun
{
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            photonView.RPC("RPCDestroyObject", RpcTarget.All,
                gameObject.GetComponent<PhotonView>().ViewID, 3f);
            // Destroy(gameObject, 3);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            photonView.RPC("RPCDestroyObject", RpcTarget.All,
                gameObject.GetComponent<PhotonView>().ViewID, 3f);
            // Destroy(gameObject, 3);
        }
        else if(other.gameObject.tag == "Wall")
        {
            // RPC 구현 요구
            photonView.RPC("RPCDestroyObject", RpcTarget.All,
                gameObject.GetComponent<PhotonView>().ViewID);
            //Destroy(gameObject);
        }
    }


    [PunRPC]
    private void RPCDestroyObject(int viewID)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        Destroy(obj);
    }
    [PunRPC]
    private void RPCDestroyObject(int viewID, float time)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        Destroy(obj,time);
    }
}
