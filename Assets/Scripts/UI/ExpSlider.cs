using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    Slider slider; 
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void LateUpdate() {
        slider.value = (int)GameManager.Instance.Exp;
        slider.maxValue = (int)GameManager.Instance.MaxExp;
    }
}
