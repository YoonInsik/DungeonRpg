using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : Singleton<MenuUI>
{
    public GameObject settingButton;
    public GameObject quitButton;
    public GameObject continueButton;
    public GameObject menuWindow;
    public GameObject settingUI;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        //settingButton.GetComponent<Button>().onClick.AddListener(SettingButton);
    }

    public void openMenuUI()
    {
        gameObject.SetActive(true);
        settingUI.SetActive(false);
        menuWindow.SetActive(true);
    }

    // start 씬에서의 설정버튼
    public void openSettingUI()
    {
        gameObject.SetActive(true);
        settingUI.SetActive(true);
        if (menuWindow.activeSelf)
        {
            Debug.Log("UI열림");
        }
        Debug.Log(menuWindow.activeSelf.ToString());
    }

    public void SettingButton()
    {
        settingUI.SetActive(true);
        //menuWindow.SetActive(false);
    }

    public void Quitbutton()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayButton()
    {
        gameObject.SetActive(false);
    }
}
