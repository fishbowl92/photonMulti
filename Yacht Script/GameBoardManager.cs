using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    Vector3 diceVelocity;

    private void OnTriggerStay(Collider other)
    {
        Vector3 diceVelocity = other.transform.parent.GetComponentInParent<Rigidbody>().velocity;
        if(diceVelocity.x == 0 && diceVelocity.y == 0 && diceVelocity.z == 0)
        {
            switch(other.gameObject.name)
            {
                case "Side1":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 6;
                    break;
                case "Side2":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 5;
                    break;
                case "Side3":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 4;
                    break;
                case "Side4":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 3;
                    break;
                case "Side5":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 2;
                    break;
                case "Side6":
                    other.transform.parent.GetComponentInParent<DiceManager>().upSideNum = 1;
                    break;
            }
        }
        
    }
}
