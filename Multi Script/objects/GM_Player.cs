using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GM_Player : MonoBehaviourPun
{
    public Follow cam;
    public GameObject grenadeGroup;
    public Camera followCamera;

    public float speed = 15, jumpPower = 15;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject grenadesGroupPrefab;

    public int ammo, coin, health, hasGrenades;
    public int maxAmmo, maxCoin, maxHealth, maxHasGrenades;


    private float hAxis, vAxis, fireDelay;
    private bool wDown, jDown, iDown, sDown1, sDown2, sDown3, fDown, rDown;
    private bool isJumping = false, isDodging = false, 
        isSwapping = false, isFireReady = true, isReload;
    private bool isBorder;      // 경계선에 닿았나 안닿았나 체크용 플레그 변수
    private int equippedWeaponIndex = -1;
    Animator anim;
    Vector3 moveVec, dodgeVec;
    Rigidbody rigid;

    GameObject nearObject;
    Weapon equippedWeapon;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (!photonView.IsMine)
            return; 
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Follow>();
        cam.target = this.gameObject.transform;
        followCamera = cam.gameObject.GetComponent<Camera>();


        grenadeGroup = PhotonNetwork.Instantiate(grenadesGroupPrefab.name, transform.position, Quaternion.identity);
        grenadeGroup.GetComponent<FollowPlayer>().target = this.gameObject.transform;
        Transform[] allChildren = grenadeGroup.GetComponentsInChildren<Transform>();
        int index = -1;
        foreach (Transform child in allChildren)
        {
            if (index == -1)
            {
                index++;
                continue;
            }
            grenades[index] = child.GetChild(0).gameObject;
            if (index < 3) index++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 내 로컬 캐릭터가 아니면 작동 x
        if (!photonView.IsMine)
            return;

        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Reload();
        Dodge();
        Interaction();
        Swap();
        Orbit();
    }

    void GetInput()
    {
        //캐릭터 이동 wasd
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk"); // 좌측 쉬프트키
        jDown = Input.GetButtonDown("Jump"); // 스페이스바
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction"); // e키 
        sDown1 = Input.GetButtonDown("Swap1"); // 1번 키
        sDown2 = Input.GetButtonDown("Swap2"); // 2번 키
        sDown3 = Input.GetButtonDown("Swap3"); // 3번 키
    }
     
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodging)
            moveVec = dodgeVec;
        if(isSwapping || !isFireReady || isReload)
            moveVec = Vector3.zero;

        if(!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRunning", moveVec != Vector3.zero);
        anim.SetBool("isWalking", wDown);
    }

    void Turn()
    {
        // #1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);

        // #2. 마우스에 의한 회전
        if (fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJumping && !isDodging && !isSwapping)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            anim.SetTrigger("doJump");
            isJumping = true;
        }
    }

    void Attack()
    {
        if (equippedWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equippedWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodging)
        {
            equippedWeapon.Use();
            anim.SetTrigger(equippedWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equippedWeapon == null)
            return;
        if (equippedWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;

        if(rDown && !isJumping && !isDodging && !isSwapping && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < equippedWeapon.maxAmmo ? ammo : equippedWeapon.maxAmmo;
        equippedWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJumping && !isDodging && !isSwapping)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equippedWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equippedWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equippedWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJumping && !isDodging)
        {
            if (equippedWeapon != null)
                photonView.RPC("RPCSetActive", RpcTarget.All, equippedWeapon.GetComponent<PhotonView>().ViewID, false);
                //equippedWeapon.SetActive(false);

            equippedWeaponIndex = weaponIndex;
            equippedWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            photonView.RPC("RPCSetActive", RpcTarget.All,
                equippedWeapon.GetComponent<PhotonView>().ViewID, true);
            //weapons[weaponIndex].SetActive(true);

            anim.SetTrigger("doSwap");

            isSwapping = true;

            Invoke("SwapOut", 0.4f);
        }

    }

    void SwapOut()
    {
        isSwapping = false;
    }

    void Interaction()
    {
        if(iDown && nearObject!=null && !isJumping && !isDodging)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.gameObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                photonView.RPC("RPCDestroyObject", RpcTarget.All,
                    nearObject.GetComponent<PhotonView>().ViewID);
            }
        }

    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        FreezeRotation();
        StopToWall();
    }

    void Orbit()
    {
        grenadeGroup.transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
            return;

        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJumping", false);
            isJumping = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth) health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    photonView.RPC("RPCSetActive", RpcTarget.All,
                        grenades[hasGrenades].GetComponent<PhotonView>().ViewID, true);
                    //grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
                    break;
            }
            photonView.RPC("RPCDestroyObject", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (other.tag == "Weapon")
            nearObject = null;
    }

    [PunRPC]
    private void RPCDestroyObject(int viewID)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        Destroy(obj);
    }

    [PunRPC]
    private void RPCSetActive(int viewID, bool cond)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        obj.SetActive(cond);
    }
}
