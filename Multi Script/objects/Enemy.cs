using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class Enemy : MonoBehaviourPun
{
    public int maxHealth;
    public int curHealth;

    public TextMeshPro TMP;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            Vector3 reactVec = transform.position - other.transform.position;

            photonView.RPC("RPCDamage", RpcTarget.All,
                gameObject.GetComponent<PhotonView>().ViewID, weapon.damage, "Melee", reactVec);
            
            //curHealth -= weapon.damage;
            //Debug.Log("Melee : " + curHealth);
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Vector3 reactVec = transform.position - other.transform.position;
            
            // 총알 제거
            photonView.RPC("RPCDestroyObject", RpcTarget.All,
                other.gameObject.GetComponent<PhotonView>().ViewID);

            photonView.RPC("RPCDamage", RpcTarget.All, 
                gameObject.GetComponent<PhotonView>().ViewID, bullet.damage, "Range", reactVec);

            //curHealth -= bullet.damage;
            //Debug.Log("Range : " + curHealth);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            TMP.text = "게이킹 주민우는 퇴치되었다!";
            gameObject.layer = 14;

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4);
        }
    }

    [PunRPC]
    private void RPCDamage(int viewID, int damage, string type, Vector3 reactVec)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        curHealth -= damage;
        TMP.text = type + " : " + curHealth;
        StartCoroutine(OnDamage(reactVec));
    }

    [PunRPC]
    private void RPCDestroyObject(int viewID)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        GameObject obj = PhotonView.Find(viewID).gameObject;
        Destroy(obj);
    }
}
