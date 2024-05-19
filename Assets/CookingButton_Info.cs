using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookingButton_Info : MonoBehaviour
{
    [SerializeField] GameObject Info;

    [SerializeField] TextMeshProUGUI txt;

    public void Set_Text(string _txt)
    {
        txt.text = _txt;
    }

    public void Open()
    {
        Info.SetActive(true);
    }

    public void Close()
    {
        Info.SetActive(false);
    }

}
