using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertText : MonoBehaviour
{
    CanvasRenderer canvas;
    public GameObject textPrefab;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasRenderer>();
        StartCoroutine(TextFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TextFade()
    {
        float startTime = 0.0f;
        float fadeTime = 2.0f;
        while (startTime <= fadeTime)
        {
            canvas.SetAlpha(Mathf.Lerp(1f, 0f, startTime / fadeTime));
            startTime += Time.deltaTime;
            yield return null;
        }
    }

    public void InstantiateAlert(string text)
    {
        if(transform.childCount >= 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        TextMeshProUGUI alertText = textPrefab.GetComponent<TextMeshProUGUI>();
        alertText.text = text;
        Instantiate(textPrefab, gameObject.transform);

        Debug.Log("alert text");
    }
}
