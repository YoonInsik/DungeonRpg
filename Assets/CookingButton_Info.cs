using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookingButton_Info : MonoBehaviour, IScrollHandler
{
    ScrollRect scrollRect;

    [SerializeField] GameObject Info;

    [SerializeField] TextMeshProUGUI txt;

    private void Start()
    {
        scrollRect = gameObject.transform.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void Set_Text(string _txt)
    {
        txt.text = _txt;
    }

    public void Open()
    {
        Info.SetActive(true);
    }

    public void Close()
    {
        Info.SetActive(false);
    }

    public void OnScroll(PointerEventData eventData)
    {
        scrollRect.OnScroll(eventData);
    }
}
