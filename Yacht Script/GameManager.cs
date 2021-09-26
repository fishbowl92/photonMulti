using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject dice1, dice2, dice3, dice4, dice5;
    private List<DiceManager> diceM = new List<DiceManager>();
    public GameObject scoreBoard;
    public int[] diceResults = new int[5];
    public List<int> diceResult = new List<int>();

    public ScoreBoardManager sbm;

    private bool sorting = false;       // true일때 주사위를 정렬시킨다.
    private bool setDiceScale = false;  // true일때 주사위의 크기를 변경시킨다.
    public List<GameObject> rolledDice = new List<GameObject>();

    public bool[] storedSpaceFlag = new bool[5];    // 저장공간이 비어있는지 체크하는 용도로 쓰인다.

    public bool rollingPhase = true;        // 굴리는 페이즈
    public bool selectingPhase = false;      // 주사위 선택가능을 나타냄
    public bool selectingScorePhase = false;      // 점수 선택 가능

    public GameObject UIRollButton;

    public GameObject[] Player = new GameObject[4];

    public int playerNum;   // 플레이어 숫자

    public GameObject mainMenu; // 메인화면 게임오브젝트

    public int playerTurn;  // 현재 주사위를 굴릴 플레이어 표시
    public int getCumTurn;  // 점수판 누른 누적 횟수 ( 게임 종료시 활용 )
    public GameObject[] scoreBoardImage = new GameObject[3];    // 점수판 이미지

    public Button rollButton;   // 굴리기 버튼
    public int rollCnt = 3;     // 굴리기 횟수 체크

    // 사운드
    public AudioClip diceSound;
    AudioSource audioSource;

    // 우승자 출력
    public GameObject WinnerPanel;
    public TextMeshProUGUI WinnerText;

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
            storedSpaceFlag[i] = false;

        rolledDice.Add(dice1);
        rolledDice.Add(dice2);
        rolledDice.Add(dice3);
        rolledDice.Add(dice4);
        rolledDice.Add(dice5);

        for(int i = 0; i<rolledDice.Count; i++)
        {
            diceM.Add(rolledDice[i].transform.GetComponent<DiceManager>());
        }

        playerTurn = 1;     // 주사위를 첫번째 플레이어가 굴릴수 있도록 초기화
        mainMenu.SetActive(true);
        for(int i = 0; i < 3; i++)
        {
            scoreBoardImage[i].SetActive(false);
        }
        getCumTurn = 1;


    }

    // Update is called once per frame
    void Update()
    {
        if (sorting == true)
        {
            SortDiceToPlayer();
            
        }
        if(setDiceScale == true)
        {
            SetBigDiceScale();
        }
    }

    // 주사위 굴리기
    public void Roll()
    {
        rollCnt--;  // 굴릴때마다 굴릴수 있는 횟수 1씩 감소
        sbm.DisableAllButtons();    // 주사위 굴릴떄 모든 버튼 비활성화
        if (rollCnt == 0)
        {
            rollButton.interactable = false;
        }

        // rolledDice를 비우고 저장되지 않은 주사위만 모아서 굴린다.
        InitRolledDice();
        if (rollingPhase)
        {
            if (selectingPhase)
            {
                InitRollDice();
                selectingPhase = false;
            }

            for (int i = 0; i < rolledDice.Count; i++)
            {
                rolledDice[i].SendMessage("Roll");
            }

            // 굴려진 주사위만 모은다.
            // 이후 5개의 주사위 값을 받는다.
            Invoke("GetNum", 3.5f);

            rollingPhase = false;
        }

        // 굴리기 버튼 눌렀을때 버튼 안보이게 만들기 ( 존재는함 )
        rollButton.image.color = new Color(0, 0, 0, 0);
        rollButton.interactable = false;
        // 사운드 출력
        audioSource.clip = diceSound;
        audioSource.Play();
    }

    // rolledDice를 비우고 저장되지 않은 주사위만 모아서 굴린다.
    public void InitRolledDice()
    {
        rolledDice.Clear();
        for (int i = 0; i < 5; i++)
        {
            diceM[i].locationBeforeStored = new Vector3(-17, 5, 0);
            if (diceM[i].isStored == false)
            {
                rolledDice.Add(diceM[i].transform.gameObject);
            }
        }
    }


    // 주사위 굴리기전 좌표 초기화 및 상태 초기화
    public void InitRollDice()
    {
        for (int i = 0; i < rolledDice.Count; i++)
        {
            // 주사위 크기 원위치
            rolledDice[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            // 물리값을 가짐
            rolledDice[i].GetComponent<Rigidbody>().isKinematic = false;
            // 회전각 정렬
            rolledDice[i].transform.rotation = Quaternion.Euler(rolledDice[i].transform.rotation.eulerAngles.x, 0, rolledDice[i].transform.rotation.eulerAngles.z);
            // 주사위 뿌리기 x : -13~-8, y : 1~5, z : -3~3
            rolledDice[i].transform.position = new Vector3(Random.Range(-13.0f, -8.0f), Random.Range(1.0f, 5.0f), Random.Range(-3.0f, 3.0f));
        }
    }

    // 주사위의 윗면 값을 알기위한 함수
    public void GetNum()
    {
        // diceResult는 int list, 주사위 눈 값 저장용
        diceResult.Clear();

        for (int i = 0; i < rolledDice.Count; i++)
        {
            rolledDice[i].GetComponent<DiceManager>().FixDiceTopNum();
            diceResult.Add(rolledDice[i].GetComponent<DiceManager>().upSideNum);
        }

        // 점수체크용
        scoreBoard.transform.GetComponent<ScoreBoardManager>().GetResults(playerNum, playerTurn);

        sorting = true;
        setDiceScale = true;

        SortNumOfDice();
    }

    // 굴려진 주사위의 숫자 결과 정렬
    public void SortNumOfDice()
    {
        GameObject temp;
        int tempR;

        
        for (int i = 0; i < rolledDice.Count-1; i++)
        {
            for(int j = i + 1; j< rolledDice.Count;j++)
            {
                if (diceResult[i] > diceResult[j])
                {
                    temp = rolledDice[i];
                    rolledDice[i] = rolledDice[j];
                    rolledDice[j] = temp;

                    tempR = diceResult[i];
                    diceResult[i] = diceResult[j];
                    diceResult[j] = tempR;
                }
                else if (diceResult[i] > diceResult[j])
                {

                }
            }
        }
    }

    // 굴려진 주사위의 결과를 플레이어에게 보여주기위하여 위치 정렬
    public void SortDiceToPlayer()
    {
        float moveSpeed = 30f;
        // 게임판 좌표 -11 0 0

        if (rolledDice.Count != 0)
        {
            // 주사위 개수에 따라, 이동 좌표값 변경
            for (int i = 0; i < rolledDice.Count; i++)
            {
                rolledDice[i].GetComponent<Rigidbody>().isKinematic = true;
                rolledDice[i].transform.position = Vector3.MoveTowards(rolledDice[i].transform.position,
                        new Vector3((-10 - rolledDice.Count) + 2 * i, 5, 0), moveSpeed * Time.deltaTime);

                // rotation 4:0,0,0 , 2:-90,0,0,  3:-180,0,0, 5:90,0,0, 1:0,0,90, 6:0,0,-90
                switch (diceResult[i])
                {
                    case 1:
                        rolledDice[i].transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 2:
                        rolledDice[i].transform.rotation = Quaternion.Euler(-90, 0, 0);
                        break;
                    case 3:
                        rolledDice[i].transform.rotation = Quaternion.Euler(-180, 0, 0);
                        break;
                    case 4:
                        rolledDice[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 5:
                        rolledDice[i].transform.rotation = Quaternion.Euler(90, 0, 0);
                        break;
                    case 6:
                        rolledDice[i].transform.rotation = Quaternion.Euler(0, 0, -90);
                        break;

                }

            }

            for (int i = 0; i < rolledDice.Count; i++)
            {
                // 스위치문은 사서 드세요...제발
                switch (rolledDice.Count)
                {
                    case 1:
                        if (rolledDice[0].transform.position == new Vector3(-11, 5, 0))
                        {
                            sorting = false;
                            // 각 주사위의 sorting된 장소를 저장함 (후에 되돌릴때 제자리 찾아가기 위해)
                            for (int j = 0; j < rolledDice.Count; j++)
                            {
                                rolledDice[j].GetComponent<DiceManager>().locationBeforeStored
                                    = rolledDice[j].transform.position;
                            }
                        }
                        break;
                    case 2:
                        if (rolledDice[0].transform.position == new Vector3(-12, 5, 0) &&
                            rolledDice[1].transform.position == new Vector3(-10, 5, 0))
                        {
                            sorting = false;
                            // 각 주사위의 sorting된 장소를 저장함 (후에 되돌릴때 제자리 찾아가기 위해)
                            for (int j = 0; j < rolledDice.Count; j++)
                            {
                                rolledDice[j].GetComponent<DiceManager>().locationBeforeStored
                                    = rolledDice[j].transform.position;
                            }
                        }
                        break;
                    case 3:
                        if (rolledDice[0].transform.position == new Vector3(-13, 5, 0) &&
                            rolledDice[1].transform.position == new Vector3(-11, 5, 0) &&
                            rolledDice[2].transform.position == new Vector3(-9, 5, 0))
                        {
                            sorting = false;
                            // 각 주사위의 sorting된 장소를 저장함 (후에 되돌릴때 제자리 찾아가기 위해)
                            for (int j = 0; j < rolledDice.Count; j++)
                            {
                                rolledDice[j].GetComponent<DiceManager>().locationBeforeStored
                                    = rolledDice[j].transform.position;
                            }
                        }
                        break;
                    case 4:
                        if (rolledDice[0].transform.position == new Vector3(-14, 5, 0) &&
                            rolledDice[1].transform.position == new Vector3(-12, 5, 0) &&
                            rolledDice[2].transform.position == new Vector3(-10, 5, 0) &&
                            rolledDice[3].transform.position == new Vector3(-8, 5, 0))
                        {
                            sorting = false;
                            // 각 주사위의 sorting된 장소를 저장함 (후에 되돌릴때 제자리 찾아가기 위해)
                            for (int j = 0; j < rolledDice.Count; j++)
                            {
                                rolledDice[j].GetComponent<DiceManager>().locationBeforeStored
                                    = rolledDice[j].transform.position;
                            }
                        }
                        break;
                    case 5:
                        if (rolledDice[0].transform.position == new Vector3(-15, 5, 0) &&
                            rolledDice[1].transform.position == new Vector3(-13, 5, 0) &&
                            rolledDice[2].transform.position == new Vector3(-11, 5, 0) &&
                            rolledDice[3].transform.position == new Vector3(-9, 5, 0) &&
                            rolledDice[4].transform.position == new Vector3(-7, 5, 0))
                        {
                            sorting = false;
                            // 각 주사위의 sorting된 장소를 저장함 (후에 되돌릴때 제자리 찾아가기 위해)
                            for (int j = 0; j < rolledDice.Count; j++)
                            {
                                rolledDice[j].GetComponent<DiceManager>().locationBeforeStored
                                    = rolledDice[j].transform.position;
                            }
                        }
                        break;
                }

            }
        }


    }

    //굴려진 주사위 보여주기식 확대 후 선택가능
    public void SetBigDiceScale()
    {
        // 주사위 갯수 상관없이 실행 가능
        if (rolledDice.Count != 0)
        {
            for (int i = 0; i < rolledDice.Count; i++)
            {
                rolledDice[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            if (rolledDice[0].transform.localScale.x >= 1.2)
            {
                Invoke("SetRollingPhase", 0f);
                selectingPhase = true;
                setDiceScale = false;
                // 정렬됬을때 버튼을 다시 보이게 설정
                rollButton.image.color = new Color(255, 255, 255, 255);
                if (rollCnt > 0)
                    rollButton.interactable = true;
                selectingScorePhase = true; // 점수 선택 가능해짐
            }
        }
    }

    // rolling Phase 1초뒤 실행되는 용도
    public void SetRollingPhase()
    {
        rollingPhase = true;
    }

    // 2인용 선택
    public void Select2Player()
    {
        playerNum = 2;
        mainMenu.SetActive(false);
        scoreBoardImage[0].SetActive(true);
    }
    // 3인용 선택
    public void Select3Player()
    {
        playerNum = 3;
        mainMenu.SetActive(false);
        scoreBoardImage[1].SetActive(true);
    }
    // 4인용 선택
    public void Select4Player()
    {
        playerNum = 4;
        mainMenu.SetActive(false);
        scoreBoardImage[2].SetActive(true);
    }

    // 점수를 선택했을때 호출
    public void SetNextPlayerTurn()
    {
        /*
        첫번째 플레이어 부터 굴림
        점수 선택시 다음 플레이어 턴으로 넘기기
        12회 터치 이후 게임결과 출력
         */
        ++playerTurn;
        ++getCumTurn;
        rollCnt = 3;
        rollButton.interactable = true;
        if (playerTurn > playerNum)
        {
            playerTurn = 1;
        }
        
    }

    // 점수 결과를 보여준다. 게임 완전히 끝나게 하는 함수의 역할도 수행 예정
    public void GetSumResultText        ()
    {
        switch (playerNum)
        {
            case 2:
                if (getCumTurn == 24)
                {
                    // 종료하는 함수 시행
                    sbm._2playersScoreText[13].text = "" + sbm.player[0].GetComponent<PlayerManager>().GetTotal();
                    sbm._2playersScoreText[13].color = new Color(0, 0, 0, 255);
                    sbm._2playersScoreText[27].text = "" + sbm.player[1].GetComponent<PlayerManager>().GetTotal();
                    sbm._2playersScoreText[27].color = new Color(0, 0, 0, 255);
                    SelectWinner();
                }
                break;
            case 3:
                if (getCumTurn == 36)
                {
                    // 종료하는 함수 시행
                    sbm._3playersScoreText[13].text = "" + sbm.player[0].GetComponent<PlayerManager>().GetTotal();
                    sbm._3playersScoreText[13].color = new Color(0, 0, 0, 255);
                    sbm._3playersScoreText[27].text = "" + sbm.player[1].GetComponent<PlayerManager>().GetTotal();
                    sbm._3playersScoreText[27].color = new Color(0, 0, 0, 255);
                    sbm._3playersScoreText[41].text = "" + sbm.player[2].GetComponent<PlayerManager>().GetTotal();
                    sbm._3playersScoreText[41].color = new Color(0, 0, 0, 255);
                    SelectWinner();
                }
                break;
            case 4:
                if (getCumTurn == 48)
                {
                    // 종료하는 함수 시행
                    sbm._4playersScoreText[13].text = "" + sbm.player[0].GetComponent<PlayerManager>().GetTotal();
                    sbm._4playersScoreText[13].color = new Color(0, 0, 0, 255);
                    sbm._4playersScoreText[27].text = "" + sbm.player[1].GetComponent<PlayerManager>().GetTotal();
                    sbm._4playersScoreText[27].color = new Color(0, 0, 0, 255);
                    sbm._4playersScoreText[41].text = "" + sbm.player[2].GetComponent<PlayerManager>().GetTotal();
                    sbm._4playersScoreText[41].color = new Color(0, 0, 0, 255);
                    sbm._4playersScoreText[55].text = "" + sbm.player[3].GetComponent<PlayerManager>().GetTotal();
                    sbm._4playersScoreText[55].color = new Color(0, 0, 0, 255);
                    SelectWinner();
                }
                break;
        }


    }

    public void SelectWinner()
    {
        int[] scores = new int[4];
        scores[0] = sbm.player[0].GetComponent<PlayerManager>().GetTotal();
        scores[1] = sbm.player[1].GetComponent<PlayerManager>().GetTotal();
        scores[2] = -100;
        scores[3] = -100;
        if (playerNum >= 3)
            scores[2] = sbm.player[2].GetComponent<PlayerManager>().GetTotal();
        if (playerNum == 4)
            scores[3] = sbm.player[3].GetComponent<PlayerManager>().GetTotal();
        int maxValue = scores.Max();
        int maxIndex = scores.ToList().IndexOf(maxValue) + 1;

        WinnerPanel.SetActive(true);
        if (scores.Max() == scores[0] && scores.Max() == scores[1] ||
            scores.Max() == scores[0] && scores.Max() == scores[2] ||
            scores.Max() == scores[0] && scores.Max() == scores[3] ||
            scores.Max() == scores[1] && scores.Max() == scores[2] ||
            scores.Max() == scores[1] && scores.Max() == scores[3] ||
            scores.Max() == scores[2] && scores.Max() == scores[3])
            WinnerText.text = "Draw !!";
        else
            WinnerText.text = "Player" + maxIndex.ToString() + " won the game!";

    }

    public void ReGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameObject.scene.name);
    }
}
