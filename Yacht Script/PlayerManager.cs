using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어의 모든 정보를 담고있다 - 이름, 점수들
public class PlayerManager : MonoBehaviour
{
    public string playerName;
    public int aces = -1, deuces = -1, threes = -1, fours = -1, fives = -1, sixes = -1,
        choice = -1, fourOfaKind = -1, fullHouse = -1,
        smallS = -1, largeS = -1, yacht = -1, sum = 0, subtotal = -1;

    public bool[] savedScore = new bool[12];
    public bool isUpperBoardFinished = false;
    public int bonus;    // 위 1,2,3,4,5,6이 다찻을때, 보너스 점수 유무 체크


    public void Start()
    {
        for (int i = 0; i < savedScore.Length; i++)
            savedScore[i] = false;
        aces = -1; deuces = -1; threes = -1; fours = -1; fives = -1; sixes = -1;
        choice = -1; fourOfaKind = -1; fullHouse = -1;
        smallS = -1; largeS = -1; yacht = -1; sum = 0; subtotal = -1;
    }
    public void SaveScore(string type, int score)
    {
        switch (type)
        {
            case "aces":
                aces = score;
                break;
            case "deuces":
                deuces = score;
                break;
            case "threes":
                threes = score;
                break;
            case "fours":
                fours = score;
                break;
            case "fives":
                fives = score;
                break;
            case "sixes":
                sixes = score;
                break;
            case "choice":
                choice = score;
                break;
            case "fourOfaKind":
                fourOfaKind = score;
                break;
            case "fullHouse":
                fullHouse = score;
                break;
            case "smallS":
                smallS = score;
                break;
            case "largeS":
                largeS = score;
                break;
            case "yacht":
                yacht = score;
                break;
        }
        isUpperBoardFinished = (aces >= 0 && deuces >= 0 && threes >= 0 && fours >= 0 && fives >= 0 && sixes >= 0);
    }

    // 서브 토탈 계산
    public void GetSubTotal()
    {
        if (isUpperBoardFinished)
        {
            subtotal = aces + deuces + threes + fours + fives + sixes;
            if (subtotal >= 63)
            {
                bonus = 35;
            }
            else
                bonus = 0;
        }
    }
    // 토탈 계산
    public int GetTotal()
    {
        Debug.Log(subtotal + "+" + choice + "+" + fourOfaKind + "+" + fullHouse + "+" + smallS + "+" + largeS + "+" + yacht + "+" + bonus);
        sum = subtotal + choice + fourOfaKind + fullHouse + smallS + largeS + yacht + bonus;
        Debug.Log("Total = " + sum);
        return sum;

    }
}
