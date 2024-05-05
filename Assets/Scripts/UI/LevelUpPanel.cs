using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private List<ItemSelectPanel> options;

    private void Start()
    {
        SetActiveOptions(false);
    }

    public void PopUpLevelUpPanel()
    {
        if (GameManager.Instance.levelUpAmount <= 0) return;
        GameManager.Instance.levelUpAmount--;

        GameManager.Instance.SelectLevelUpItems(options.Count);
        background.enabled = true;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].gameObject.SetActive(true);
            options[i].SetUI(i);
        }
    }

    public void SetActiveOptions(bool value)
    {
        background.enabled = value;
        foreach (var option in options)
        {
            option.gameObject.SetActive(value);
        }
    }
}
