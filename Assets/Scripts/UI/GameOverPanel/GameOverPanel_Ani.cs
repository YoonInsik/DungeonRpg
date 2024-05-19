using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel_Ani : MonoBehaviour
{
    [SerializeField] GameObject btn_1;
    [SerializeField] GameObject btn_2;

    void Ani_Button_On()
    {
        btn_1.SetActive(true);
        btn_2.SetActive(true);
    }
}
