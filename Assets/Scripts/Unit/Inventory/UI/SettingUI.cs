using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject saveButton;
    public GameObject cancelButton;
    public GameObject menuWindow;
    public GameObject[] settingUIs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CancelButton()
    {
        gameObject.SetActive(false);
        menuWindow.SetActive(true);
    }

    public void SaveButton()
    {
        gameObject.SetActive(false);
        menuWindow.SetActive(true);
    }
}
