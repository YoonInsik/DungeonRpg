using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public GameObject saveButton;
    public GameObject cancelButton;
    public GameObject menuWindow;
    //public GameObject[] settingUIs;

    public bool fullScreen;
    public Toggle toggle;
    public TMP_Dropdown dropdown;

    public List<Resolution> resolutions = new List<Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        InitDropDown();
        InitToggle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitDropDown()
    {
        resolutions.AddRange(Screen.resolutions);
        //중복제거
        resolutions = resolutions.Distinct().ToList();
        dropdown.options.Clear();

        for (int i = 0; i < resolutions.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "Hz"
            };
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                dropdown.SetValueWithoutNotify(i);
            }
            dropdown.options.Add(option);
        }

        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, fullScreen);
    }

    public void InitToggle()
    {
        fullScreen = Screen.fullScreen;
        toggle.isOn = fullScreen;
        toggle.onValueChanged.AddListener(SetFullScreen);
    }
    public void SetFullScreen(bool fullScreen)
    {
        this.fullScreen = fullScreen;
        Screen.fullScreen = fullScreen;
    }

    public void CancelButton()
    {
        gameObject.SetActive(false);
        menuWindow.SetActive(true);
        Debug.Log(Screen.height + " x " + Screen.width);
        Debug.Log(Screen.fullScreen);
    }

    public void SaveButton()
    {
        gameObject.SetActive(false);
        menuWindow.SetActive(true);
        Debug.Log(Screen.width + " x " + Screen.height);
        Debug.Log(Screen.fullScreen);
    }

    
}
