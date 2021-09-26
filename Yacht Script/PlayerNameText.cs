using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerNameText : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    // Start is called before the first frame update
    void Start()
    {
        nameText = GetComponent<TextMeshProUGUI>();
        if(AuthManager.User != null)
        {
            nameText.text = $"Hi! {AuthManager.User.Email}";
        }
        else
        {
            nameText.text = "ERROR: AuthManager.User == NULL";
        }
    }

}
