using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI levelText;

    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();   
    }

    void LateUpdate()
    {
        levelText.text = GameManager.Instance.Level.ToString();   
    }
}
