using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameManager gm;
    static Rigidbody rb;
    public static Vector3 diceVelocity;
    public static Vector3 eulers;
    public int upSideNum;
    public bool isRolling;          // 현재 주사위를 굴릴수 있는지를 나타낸다
    private float _topPositionSide; // 최상위 주사위 면의 y 좌표
        
    public bool isStored = false;   // 저장된 상태를 나타냄
    public bool moveToStore = false;   // 저장공간으로 이동하는 권한을 가진다
    public bool moveToBoard = false;   // 보드공간으로 이동하는 권한을 가진다.
    public int goToThisSpace = -1;     // 어떤 저장공간으로 갈지를 정해준다.

    public Vector3 locationBeforeStored; // 저장되기전 원래 있던 위치

    public bool isBackToBoard = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isRolling = true;
        _topPositionSide = -100;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToStore == true)        // 저장공간으로 이동이 가능한 상태인 경우
        {
            MoveToStore(goToThisSpace); // 부여받은 저장공간으로 이동을 실행
        }
        if (moveToBoard == true)
        {
            MoveToBoard();
            isBackToBoard = false;
        }

        diceVelocity = rb.velocity;

        if (diceVelocity.x == 0 && diceVelocity.y == 0 && diceVelocity.z == 0)
            isRolling = false;
        else
            isRolling = true;
    }

    public void FixDiceTopNum()
    {
        Transform[] _allChildren = GetComponentsInChildren<Transform>();    // 주사위의 모든 면을 불러온다
        string sideName = "";
        foreach (Transform child in _allChildren)    // 모든 면의 주사위를 체크
        {
            if (child.name == transform.name)   // 자기 자신일 경우 제외한다
                continue;
            if (_topPositionSide < child.transform.position.y)   // 앞의 결과와 비교하여 자신이 최상위 y 값을 갖는지 체크
            {
                _topPositionSide = child.transform.position.y;
                sideName = child.name;
            }
        }

        switch (sideName)
        {
            case "Side1":
                upSideNum = 1;
                break;
            case "Side2":
                upSideNum = 2;
                break;
            case "Side3":
                upSideNum = 3;
                break;
            case "Side4":
                upSideNum = 4;
                break;
            case "Side5":
                upSideNum = 5;
                break;
            case "Side6":
                upSideNum = 6;
                break;
        }
    }

    // 마우스 클릭이 입력될때 실행된다.
    public void OnMouseDown()
    {
        // 선택가능한 상태인 경우
        if (gm.selectingPhase == true)
        {
            // 현재 저장되어있지 않은 상태이고, 선택가능 상태라면
            if (isStored == false && gm.selectingPhase == true)
            {
                this.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);  // 주사위 크기를 줄이고
                isStored = true;        // 저장된 상태로 바꾼다
                StoreDice();            // 주사위가 어느 공간에 저장시킬지를 정한다.
            }
            // 현재 저장되어있는 상태이고, 선택가능 상태라면
            else if (isStored == true && gm.selectingPhase == true)
            {
                this.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);  // 주사위 크기를 키우고
                isStored = false;       // 저장된 상태에서 해방시킨다.
                moveToBoard = true;      // 보드로 돌아갈수 있는 상태로 만듬
                gm.storedSpaceFlag[goToThisSpace] = false;  // 저장되어 있던 자리를 비우고
                goToThisSpace = -1; // 저장해야 할 곳도 초기화 시킨다.
            }
        }

    }

    // 선택된 주사위를 해당되는 저장공간에 설정
    public void StoreDice()
    {
        if(gm.storedSpaceFlag[0] == false)  // 0번째 저장공간이 비어있다면
        {
            goToThisSpace = 0;  // 0을 부여해서 어느 저장공간을 향해 가야하는지 설정해준다.
            gm.storedSpaceFlag[0] = true;   // 0번째 저장공간이 할당되었기에 다른 주사위가 들어오는것을 막는다
            moveToStore = true; // 주사위가 저장공간으로 이동하는 권한을 준다
        }
        else if (gm.storedSpaceFlag[1] == false)  // 1번째 저장공간이 비어있다면
        {
            goToThisSpace = 1;
            gm.storedSpaceFlag[1] = true;
            moveToStore = true;
        }
        else if (gm.storedSpaceFlag[2] == false)  // 2번째 저장공간이 비어있다면
        {
            goToThisSpace = 2;
            gm.storedSpaceFlag[2] = true;
            moveToStore = true;
        }
        else if (gm.storedSpaceFlag[3] == false)  // 3번째 저장공간이 비어있다면
        {
            goToThisSpace = 3;
            gm.storedSpaceFlag[3] = true;
            moveToStore = true;
        }
        else if (gm.storedSpaceFlag[4] == false)  // 4번째 저장공간이 비어있다면
        {
            goToThisSpace = 4;
            gm.storedSpaceFlag[4] = true;
            moveToStore = true;
        }

    }

    // 주사위를 저장 공간으로 보내기
    public void MoveToStore(int where)
    {
        float moveSpeed = 50f;  // 저장공간으로 움직이는 속도 ( 더 빠를 필요가 있다 )
        gm.InitRolledDice();
        gm.GetNum();
        // 1 : -5.25,0,4,  2: -5.25,0,2, ...
        // 부여받은 번호에 따라 맞는 저장공간으로 이동한다.
        switch (where)
        {
            case 0:
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-5.25f, 1, 4), moveSpeed * Time.deltaTime);
                break;
            case 1:
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-5.25f, 1, 2), moveSpeed * Time.deltaTime);
                break;
            case 2:
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-5.25f, 1, 0), moveSpeed * Time.deltaTime);
                break;
            case 3:
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-5.25f, 1, -2), moveSpeed * Time.deltaTime);
                break;
            case 4:
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-5.25f, 1, -4), moveSpeed * Time.deltaTime);
                break;
        }
        // 저장공간에 도착했을때, 저장공간으로 이동하는 권한을 비활성화 시킨다.
        if (this.transform.position.x >= -5.25f)
        {
            moveToStore = false;
        }
    }

    // 주사위를 굴릴수 있는 보드로 보내기
    public void MoveToBoard()
    {
        if(!isBackToBoard)
        {
            isStored = false;
            gm.InitRolledDice();
            gm.GetNum();
            isBackToBoard = true;
            moveToBoard = false;
        }

    }
}
