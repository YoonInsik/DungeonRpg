using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject go_Panel;
    [SerializeField] Animator panel_ani;

    public void Object_On(bool clear)
    {
        if(clear)
            panel_ani.SetTrigger("clear");
        else
            panel_ani.SetTrigger("gameover");

        go_Panel.SetActive(true);
    }

    public void Btn_Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void Btn_Title()
    {
        SceneManager.LoadScene("Start");
    }
}
