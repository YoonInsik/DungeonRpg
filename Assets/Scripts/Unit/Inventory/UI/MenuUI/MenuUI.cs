using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        gameObject.SetActive(false);
        //settingButton.GetComponent<Button>().onClick.AddListener(SettingButton);
    }

    public void openMenuUI()
    {
        gameObject.SetActive(true);
        settingUI.SetActive(false);
        menuWindow.SetActive(true);
    }

    public void SettingButton()
    {
        settingUI.SetActive(true);
        menuWindow.SetActive(false);
    }

    public void Quitbutton()
    {

    }

    public void PlayButton()
    {

    }
}
