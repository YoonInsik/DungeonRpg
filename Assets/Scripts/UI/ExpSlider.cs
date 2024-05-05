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
        slider.value = GameManager.Instance.Exp;
        slider.maxValue = GameManager.Instance.MaxExp;
    }
}
