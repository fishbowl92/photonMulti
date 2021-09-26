using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{
    public GameManager gm;
    private int[] results = new int[5];
    public GameObject[] player = new GameObject[4]; // 플레이어 1,2,3,4
    private int curPlayer = 0; // 현재 플레이어를 가리킴

    public Button[] _2playersScoreButton = new Button[28];
    public Button[] _3playersScoreButton = new Button[42];
    public Button[] _4playersScoreButton = new Button[56];

    public TextMeshProUGUI[] _2playersScoreText = new TextMeshProUGUI[28];
    public TextMeshProUGUI[] _3playersScoreText = new TextMeshProUGUI[42];
    public TextMeshProUGUI[] _4playersScoreText = new TextMeshProUGUI[56];

    public TextMeshProUGUI[] _2playerBonusText = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] _3playerBonusText = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] _4playerBonusText = new TextMeshProUGUI[4];


    public GameObject dice1, dice2, dice3, dice4, dice5;

    private bool[] fixedScore2Players = new bool[28];
    private bool[] fixedScore3Players = new bool[42];
    private bool[] fixedScore4Players = new bool[56];

    private int playerTurn = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetPlayersMode();

        for (int i = 0; i < fixedScore2Players.Length; i++)
            fixedScore2Players[i] = false;
        for (int i = 0; i < fixedScore3Players.Length; i++)
            fixedScore3Players[i] = false;
        for (int i = 0; i < fixedScore4Players.Length; i++)
            fixedScore4Players[i] = false;

        // 버튼 색 하얀색으로 모든상황에 적용
        InitButtonColor();


    }
    
    // 주사위 결과를 가져온다.
    public void GetResults(int playerNum, int _playerTurn)
    {
        results[0] = dice1.GetComponent<DiceManager>().upSideNum;
        results[1] = dice2.GetComponent<DiceManager>().upSideNum;
        results[2] = dice3.GetComponent<DiceManager>().upSideNum;
        results[3] = dice4.GetComponent<DiceManager>().upSideNum;
        results[4] = dice5.GetComponent<DiceManager>().upSideNum;

        playerTurn = _playerTurn;
        // 굴린 점수 빨간색으로 표기 (임시)
        int index = (playerTurn - 1) * 14;

        switch (playerNum)
        {
            case 2:
                if (!fixedScore2Players[index])
                    _2playersScoreText[index].text = Aces(results).ToString();
                if (!fixedScore2Players[index + 1])
                    _2playersScoreText[index + 1].text = Deuces(results).ToString();
                if (!fixedScore2Players[index + 2])
                    _2playersScoreText[index + 2].text = Threes(results).ToString();
                if (!fixedScore2Players[index + 3])
                    _2playersScoreText[index + 3].text = Fours(results).ToString();
                if (!fixedScore2Players[index + 4])
                    _2playersScoreText[index + 4].text = Fives(results).ToString();
                if (!fixedScore2Players[index + 5])
                    _2playersScoreText[index + 5].text = Sixes(results).ToString();
                // SubSum 업데이트 필요 X
                if (!fixedScore2Players[index + 7])
                    _2playersScoreText[index + 7].text = Choice(results).ToString();
                if (!fixedScore2Players[index + 8])
                    _2playersScoreText[index + 8].text = FourOfaKind(results).ToString();
                if (!fixedScore2Players[index + 9])
                    _2playersScoreText[index + 9].text = FullHouse(results).ToString();
                if (!fixedScore2Players[index + 10])
                    _2playersScoreText[index + 10].text = SmallS(results).ToString();
                if (!fixedScore2Players[index + 11])
                    _2playersScoreText[index + 11].text = LargeS(results).ToString();
                if (!fixedScore2Players[index + 12])
                    _2playersScoreText[index + 12].text = Yacht(results).ToString();
                // Total 업데이트 필요 x
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || fixedScore2Players[index + i])
                        continue;
                    _2playersScoreText[index + i].color = new Color(255, 0, 0, 255);
                }
                break;
            case 3:
                if (!fixedScore3Players[index])
                    _3playersScoreText[index].text = Aces(results).ToString();
                if (!fixedScore3Players[index + 1])
                    _3playersScoreText[index + 1].text = Deuces(results).ToString();
                if (!fixedScore3Players[index + 2])
                    _3playersScoreText[index + 2].text = Threes(results).ToString();
                if (!fixedScore3Players[index + 3])
                    _3playersScoreText[index + 3].text = Fours(results).ToString();
                if (!fixedScore3Players[index + 4])
                    _3playersScoreText[index + 4].text = Fives(results).ToString();
                if (!fixedScore3Players[index + 5])
                    _3playersScoreText[index + 5].text = Sixes(results).ToString();
                // SubSum 업데이트 필요 X
                if (!fixedScore3Players[index + 7])
                    _3playersScoreText[index + 7].text = Choice(results).ToString();
                if (!fixedScore3Players[index + 8])
                    _3playersScoreText[index + 8].text = FourOfaKind(results).ToString();
                if (!fixedScore3Players[index + 9])
                    _3playersScoreText[index + 9].text = FullHouse(results).ToString();
                if (!fixedScore3Players[index + 10])
                    _3playersScoreText[index + 10].text = SmallS(results).ToString();
                if (!fixedScore3Players[index + 11])
                    _3playersScoreText[index + 11].text = LargeS(results).ToString();
                if (!fixedScore3Players[index + 12])
                    _3playersScoreText[index + 12].text = Yacht(results).ToString();
                // Total 업데이트 필요 x
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || fixedScore3Players[index + i])
                        continue;
                    _3playersScoreText[index + i].color = new Color(255, 0, 0, 255);
                }
                break;
            case 4:
                if (!fixedScore4Players[index])
                    _4playersScoreText[index].text = Aces(results).ToString();
                if (!fixedScore4Players[index + 1])
                    _4playersScoreText[index + 1].text = Deuces(results).ToString();
                if (!fixedScore4Players[index + 2])
                    _4playersScoreText[index + 2].text = Threes(results).ToString();
                if (!fixedScore4Players[index + 3])
                    _4playersScoreText[index + 3].text = Fours(results).ToString();
                if (!fixedScore4Players[index + 4])
                    _4playersScoreText[index + 4].text = Fives(results).ToString();
                if (!fixedScore4Players[index + 5])
                    _4playersScoreText[index + 5].text = Sixes(results).ToString();
                // SubSum 업데이트 필요 X
                if (!fixedScore4Players[index + 7])
                    _4playersScoreText[index + 7].text = Choice(results).ToString();
                if (!fixedScore4Players[index + 8])
                    _4playersScoreText[index + 8].text = FourOfaKind(results).ToString();
                if (!fixedScore4Players[index + 9])
                    _4playersScoreText[index + 9].text = FullHouse(results).ToString();
                if (!fixedScore4Players[index + 10])
                    _4playersScoreText[index + 10].text = SmallS(results).ToString();
                if (!fixedScore4Players[index + 11])
                    _4playersScoreText[index + 11].text = LargeS(results).ToString();
                if (!fixedScore4Players[index + 12])
                    _4playersScoreText[index + 12].text = Yacht(results).ToString();
                // Total 업데이트 필요 x
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || fixedScore4Players[index + i])
                        continue;
                    _4playersScoreText[index + i].color = new Color(255, 0, 0, 255);
                }
                break;
        }

        // 본인 플레이어 버튼 활성화 함수 호출
        enableButton();
        // 다른 플레이어 버튼 비활성화 함수 호출
        disableButton();
    }

    // 턴 넘길시 다음사람을 위해 주사위를 세팅한다.
    public void SettingDiceToNextPlayer()
    {
        gm.rolledDice.Clear();
        gm.rolledDice.Add(dice1);
        gm.rolledDice.Add(dice2);
        gm.rolledDice.Add(dice3);
        gm.rolledDice.Add(dice4);
        gm.rolledDice.Add(dice5);
        dice1.GetComponent<DiceManager>().isStored = false;
        dice2.GetComponent<DiceManager>().isStored = false;
        dice3.GetComponent<DiceManager>().isStored = false;
        dice4.GetComponent<DiceManager>().isStored = false;
        dice5.GetComponent<DiceManager>().isStored = false;

        dice1.GetComponent<DiceManager>().goToThisSpace = -1;
        dice2.GetComponent<DiceManager>().goToThisSpace = -1;
        dice3.GetComponent<DiceManager>().goToThisSpace = -1;
        dice4.GetComponent<DiceManager>().goToThisSpace = -1;
        dice5.GetComponent<DiceManager>().goToThisSpace = -1;

        for(int i = 0; i < 5; i++)
        {
            gm.storedSpaceFlag[i] = false;
        }
    }

    public void enableButton()
    {
        int index = (playerTurn - 1) * 14;

        for (int i = index; i < index + 14; i++)
        {
            if (gm.playerNum == 2)
            {
                if (fixedScore2Players[i])
                    _2playersScoreButton[i].interactable = false;
                else
                    _2playersScoreButton[i].interactable = true;
            }
            else if (gm.playerNum == 3)
            {
                if (fixedScore3Players[i])
                    _3playersScoreButton[i].interactable = false;
                else
                    _3playersScoreButton[i].interactable = true;
            }
            else if (gm.playerNum == 4)
            {
                if (fixedScore4Players[i])
                    _4playersScoreButton[i].interactable = false;
                else
                    _4playersScoreButton[i].interactable = true;
            }
        }
    }

    public void disableButton()
    {
        switch (playerTurn)
        {
            case 1:
                if (gm.playerNum == 2)
                {
                    // 2번째 플레이어의 버튼 비활성화
                    for (int i = 14; i < 28; i++)
                        _2playersScoreButton[i].interactable = false;
                }
                else if (gm.playerNum == 3)
                {
                    // 2 3번째 플레이어 버튼 비활성화
                    for (int i = 14; i < 42; i++)
                        _3playersScoreButton[i].interactable = false;
                }
                else if (gm.playerNum == 4)
                {
                    // 2 3 4번째 플레이어 버튼 비활성화
                    for (int i = 14; i < 56; i++)
                        _4playersScoreButton[i].interactable = false;
                }
                break;
            case 2:
                if (gm.playerNum == 2)
                {
                    // 1번째 플레이어의 버튼 비활성화
                    for (int i = 0; i < 14; i++)
                        _2playersScoreButton[i].interactable = false;
                }
                else if (gm.playerNum == 3)
                {
                    // 1 3번째 플레이어 버튼 비활성화
                    for (int i = 0; i < 14; i++)
                        _3playersScoreButton[i].interactable = false;
                    for (int i = 28; i < 42; i++)
                        _3playersScoreButton[i].interactable = false;
                }
                else if (gm.playerNum == 4)
                {
                    // 1 3 4번째 플레이어 버튼 비활성화
                    for (int i = 0; i < 14; i++)
                        _4playersScoreButton[i].interactable = false;
                    for (int i = 28; i < 56; i++)
                        _4playersScoreButton[i].interactable = false;
                }
                break;
            case 3:
                if (gm.playerNum == 3)
                {
                    // 1 2번째 플레이어 버튼 비활성화
                    for (int i = 0; i < 28; i++)
                        _3playersScoreButton[i].interactable = false;
                }
                else if (gm.playerNum == 4)
                {
                    // 1 2 4번째 플레이어 버튼 비활성화
                    for (int i = 0; i < 28; i++)
                        _4playersScoreButton[i].interactable = false;
                    for (int i = 42; i < 56; i++)
                        _4playersScoreButton[i].interactable = false;
                }
                break;
            case 4:
                // 1 2 3번째 플레이어 버튼 비활성화
                for (int i = 0; i < 42; i++)
                    _4playersScoreButton[i].interactable = false;
                break;
        }

        
    }

    // 플레이어가 고른 점수 고정
    public void fixScore(int scoreType)
    {
        int index;
        switch (gm.playerTurn)
        {
            case 1:
                index = 0;
                switch (gm.playerNum)
                {
                    case 2: //2인용 보드- 1번째 플레이어
                        fixedScore2Players[index + scoreType] = true;
                        break;
                    case 3: //3인용 보드 - 1번째 플레이어
                        fixedScore3Players[index + scoreType] = true;
                        break;
                    case 4: //4인용 보드 - 1번째 플레이어 주민우 병신
                        fixedScore4Players[index + scoreType] = true;
                        break;
                }
                break;
            case 2:
                index = 14;
                switch (gm.playerNum)
                {
                    case 2: //2인용 보드 - 2번째 플레이어
                        fixedScore2Players[index + scoreType] = true;
                        break;
                    case 3: //3인용 보드 - 2번째 플레이어
                        fixedScore3Players[index + scoreType] = true;
                        break;
                    case 4: // 4인용 보드 - 2번째 플레이어
                        fixedScore4Players[index + scoreType] = true;
                        break;
                }
                break;
            case 3:
                index = 28;
                switch (gm.playerNum)
                {
                    case 3: //3인용 보드 - 3번째 플레이어
                        fixedScore3Players[index + scoreType] = true;
                        break;
                    case 4: //4인용 보드 - 3번째 플레이어
                        fixedScore4Players[index + scoreType] = true;
                        break;
                }
                break;
            case 4:
                //무조건 4인용 보드 사용
                index = 42;
                fixedScore4Players[index + scoreType] = true;
                break;
        }
    }

    public void DisableAllButtons()
    {
        int index = gm.playerNum * 14;
        for(int i = 0;i<index;i++)
        {
            if(gm.playerNum == 2)
            {
                _2playersScoreButton[i].interactable = false;
            }
            else if (gm.playerNum == 3)
            {
                _3playersScoreButton[i].interactable = false;
            }
            else
            {
                _4playersScoreButton[i].interactable = false;
            }
        }
    }

    // 선택된 점수 색상 바꾸기
    public void SelectedTextChangeColor(int scoreType)
    {
        // 선택된 점수의 색을 검은색으로 바꿈
        int index = (playerTurn - 1) * 14;
        switch (gm.playerNum)
        {
            case 2: // 2인용 보드
                _2playersScoreText[index + scoreType].color = new Color(0, 0, 0, 255);
                break;
            case 3: // 3인용 보드
                _3playersScoreText[index + scoreType].color = new Color(0, 0, 0, 255);
                break;
            case 4: // 4인용 보드
                _4playersScoreText[index + scoreType].color = new Color(0, 0, 0, 255);
                break;
        }
        // 선택되지 않은 점수의 색을 투명으로 바꿈 ( 임시 )
        switch (gm.playerNum)
        {
            case 2:
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || i == scoreType || fixedScore2Players[index + i])
                        continue;
                    _2playersScoreText[index + i].color = new Color(0, 0, 0, 0);
                }
                break;
            case 3:
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || i == scoreType || fixedScore3Players[index + i])
                        continue;
                    _3playersScoreText[index + i].color = new Color(0, 0, 0, 0);
                }
                break;
            case 4:
                for (int i = 0; i < 13; i++)
                {
                    if (i == 6 || i == scoreType || fixedScore4Players[index + i])
                        continue;
                    _4playersScoreText[index + i].color = new Color(0, 0, 0, 0);
                }
                break;
        }


    }

    // 보너스 점수 체크
    public void CheckBonusScore()
    {
        if (player[playerTurn - 1].GetComponent<PlayerManager>().isUpperBoardFinished == true)
        {
            player[playerTurn - 1].GetComponent<PlayerManager>().GetSubTotal();
            int index = (playerTurn - 1) * 14;
            switch (gm.playerNum)
            {
                case 2: // 2인용 보드
                    _2playerBonusText[playerTurn-1].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().bonus;
                    _2playersScoreText[index + 6].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().subtotal;
                    break;
                case 3: // 3인용 보드
                    _3playerBonusText[playerTurn - 1].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().bonus;
                    _3playersScoreText[index + 6].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().subtotal;
                    break;
                case 4: // 4인용 보드
                    _4playerBonusText[playerTurn - 1].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().bonus;
                    _4playersScoreText[index + 6].text = "" + player[playerTurn - 1].GetComponent<PlayerManager>().subtotal;
                    break;
            }
        }
    }

    public void SelectAces()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("aces", Aces(results));
        fixScore(0);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(0);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectDeuces()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("deuces", Deuces(results));
        fixScore(1);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(1);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectThrees()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("threes", Threes(results));
        fixScore(2);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(2);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectFours()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("fours", Fours(results));
        fixScore(3);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(3);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectFives()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("fives", Fives(results));
        fixScore(4);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(4);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectSixes()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("sixes", Sixes(results));
        fixScore(5);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(5);
        DisableAllButtons();

        CheckBonusScore();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectChoice()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("choice", Choice(results));
        fixScore(7);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(7);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectFourOfaKind()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("fourOfaKind", FourOfaKind(results));
        fixScore(8);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(8);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectFullHouse()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("fullHouse", FullHouse(results));
        fixScore(9);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(9);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectSmallS()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("smallS", SmallS(results));
        fixScore(10);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(10);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectLargeS()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("largeS", LargeS(results));
        fixScore(11);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(11);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }
    public void SelectYacht()
    {
        player[playerTurn - 1].GetComponent<PlayerManager>().SaveScore("yacht", Yacht(results));
        fixScore(12);
        gm.GetSumResultText();
        gm.SetNextPlayerTurn();
        SelectedTextChangeColor(12);
        DisableAllButtons();
        // 주사위를 다음사람을 위해 세팅
        SettingDiceToNextPlayer();
    }

    // 점수계산
    public int Aces(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 1)
                sum += results[i];
        }
        return sum;
    }
    public int Deuces(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 2)
                sum += results[i];
        }
        return sum;
    }
    public int Threes(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 3)
                sum += results[i];
        }
        return sum;
    }
    public int Fours(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 4)
                sum += results[i];
        }
        return sum;
    }
    public int Fives(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 5)
                sum += results[i];
        }
        return sum;
    }
    public int Sixes(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (results[i] == 6)
                sum += results[i];
        }
        return sum;
    }
    public int Choice(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            sum += results[i];
        }
        return sum;
    }
    public int FourOfaKind(int[] results)
    {
        Array.Sort(results);
        if (results[0] == results[3] || results[1] == results[4])
            return Sum(results);
        return 0;
    }
    public int FullHouse(int[] results)
    {
        Array.Sort(results);
        if (results[0] == results[1] && results[2] == results[4] ||
            results[0] == results[2] && results[3] == results[4])
            return Sum(results);
        return 0;
    }
    public int SmallS(int[] results)
    {
        Array.Sort(results);
        for (int i = 0; i < 2; i++)
        {
            if (results[i + 1] == results[i] + 1)
            {
                if (results[i + 2] == results[i + 1] + 1)
                {
                    if (results[i + 3] == results[i + 2] + 1)
                    {
                        return 15;
                    }
                }
            }
        }
        return 0;
    }
    public int LargeS(int[] results)
    {
        Array.Sort(results);
        if (results[1] == results[0] + 1 &&
            results[2] == results[1] + 1 &&
            results[3] == results[2] + 1 &&
            results[4] == results[3] + 1)
            return 30;

        return 0;
    }
    public int Yacht(int[] results)
    {
        if (results[1] == results[0] &&
            results[2] == results[1] &&
            results[3] == results[2] &&
            results[4] == results[3])
            return 50;
        return 0;
    }
    public int Sum(int[] results)
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            sum += results[i];
        }
        return sum;
    }

    // 게임모드 설정
    public void SetPlayersMode()
    {
        for (int i = 0; i < _2playersScoreText.Length; i++)
        {
            _2playersScoreText[i].text = "";
        }
        for (int i = 0; i < _3playersScoreText.Length; i++)
        {
            _3playersScoreText[i].text = "";
        }
        for (int i = 0; i < _4playersScoreText.Length; i++)
        {
            _4playersScoreText[i].text = "";
        }
        for (int i = 0; i < _2playerBonusText.Length; i++)
        {
            _2playerBonusText[i].text = "";
        }
        for (int i = 0; i < _3playerBonusText.Length; i++)
        {
            _3playerBonusText[i].text = "";
        }
        for (int i = 0; i < _4playerBonusText.Length; i++)
        {
            _4playerBonusText[i].text = "";
        }
    }

    // 버튼 색 어느상황이던지 하얀색으로 초기화 시켜버리기
    public void InitButtonColor()
    {
        ColorBlock[] cb2 = new ColorBlock[_2playersScoreButton.Length];
        ColorBlock[] cb3 = new ColorBlock[_3playersScoreButton.Length];
        ColorBlock[] cb4 = new ColorBlock[_4playersScoreButton.Length];

        for (int i = 0; i < _2playersScoreButton.Length; i++)
        {
            cb2[i].normalColor = new Color(255, 255, 255, 255);
            cb2[i].highlightedColor = new Color(255, 255, 255, 255);
            cb2[i].pressedColor = new Color(255, 255, 255, 255);
            cb2[i].selectedColor = new Color(255, 255, 255, 255);
            cb2[i].disabledColor = new Color(255, 255, 255, 255);
            _2playersScoreButton[i].colors = cb2[i];
        }
        for (int i = 0; i < _3playersScoreButton.Length; i++)
        {
            cb3[i].normalColor = new Color(255, 255, 255, 255);
            cb3[i].highlightedColor = new Color(255, 255, 255, 255);
            cb3[i].pressedColor = new Color(255, 255, 255, 255);
            cb3[i].selectedColor = new Color(255, 255, 255, 255);
            cb3[i].disabledColor = new Color(255, 255, 255, 255);
            _3playersScoreButton[i].colors = cb3[i];
        }
        for (int i = 0; i < _4playersScoreButton.Length; i++)
        {
            cb4[i].normalColor = new Color(255, 255, 255, 255);
            cb4[i].highlightedColor = new Color(255, 255, 255, 255);
            cb4[i].pressedColor = new Color(255, 255, 255, 255);
            cb4[i].selectedColor = new Color(255, 255, 255, 255);
            cb4[i].disabledColor = new Color(255, 255, 255, 255);
            _4playersScoreButton[i].colors = cb4[i];
        }
    }

    
}
