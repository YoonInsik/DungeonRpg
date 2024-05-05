using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        var time = GameManager.Instance.Timer;
        timeText.text = (time < 0.01f) ? "" : string.Format("{0:F0}", time) ;
    }
}
