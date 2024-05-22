using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    public GameObject BackButton;
    public GameObject menuWindow;
    public AudioMixer AudioMixer;
    float volume;
    //public GameObject[] settingUIs;

    public bool isVSync;
    public bool isFullScreen;
    public Toggle resolution_toggle;
    public Toggle vSync_toggle;
    public Slider sound_slider;
    public TMP_Dropdown dropdown;

    //가능한 해상도를 불러와서 주사율만 제거하기 위한 구조체 및 리스트
    private struct Mod_Resolution
    {
        public int width;
        public int height;
    }
    private List<Resolution> resolutions = new List<Resolution>();
    private List<Mod_Resolution> mod_resolutions = new List<Mod_Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        InitDropDown();
        InitResolutionToggle();
        InitVSyncToggle();

        sound_slider.onValueChanged.AddListener(SetVolume);
        //AudioMixer.GetFloat("GameVolume", out volume);
        //Debug.Log(volume);
        //sound_slider.value = Mathf.Log10(volume) * 20;
    }

    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("GameVolume", Mathf.Log10(volume) * 20);
        Debug.Log(volume);
    }

    public void InitDropDown()
    {
        resolutions.AddRange(Screen.resolutions);
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width % 16 == 0 && resolution.height % 9 == 0)
            {
                Debug.Log(resolution.width / resolution.height);
                Mod_Resolution mod_Resolution = new Mod_Resolution();
                mod_Resolution.width = resolution.width;
                mod_Resolution.height = resolution.height;

                mod_resolutions.Add(mod_Resolution);
            }
        }
        mod_resolutions.Distinct().ToList();
        dropdown.options.Clear();

        for (int i = 0; i < mod_resolutions.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = mod_resolutions[i].width + " x " + mod_resolutions[i].height
            };
            if (mod_resolutions[i].width == Screen.width && mod_resolutions[i].height == Screen.height) ;
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
        Mod_Resolution resolution = mod_resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
    }

    public void InitResolutionToggle()
    {
        isFullScreen = Screen.fullScreen;
        resolution_toggle.isOn = isFullScreen;
        resolution_toggle.onValueChanged.AddListener(SetFullScreen);
    }
    public void SetFullScreen(bool fullScreen)
    {
        this.isFullScreen = fullScreen;
        Screen.fullScreen = fullScreen;
    }

    public void SaveButton()
    {
        gameObject.SetActive(false);
        Debug.Log(Screen.width + " x " + Screen.height);
        Debug.Log(Screen.fullScreen);
    }

    public void InitVSyncToggle()
    {
        QualitySettings.vSyncCount = 0;
        vSync_toggle.isOn = false;
        isVSync = false;
        vSync_toggle.onValueChanged.AddListener(SetVSync);
    }
    public void SetVSync(bool isVsync)
    {
        if (isVSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Debug.Log(QualitySettings.vSyncCount);
    }
    
}
