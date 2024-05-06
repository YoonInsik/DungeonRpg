using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSelectPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemTag;
    [SerializeField] private TextMeshProUGUI itemDesc;
    [SerializeField] private ItemData data;

    public void SetUI(int index)
    {
        data = GameManager.Instance.itemDataOptions[index];

        itemImage.sprite = data.itemImage;
        itemName.text = data.itemName;
        itemTag.text = data.itemTag.ToString();
        itemDesc.text = data.itemDesc;
    }

    public void SelectItem()
    {
        UnitManager.Instance.player.GetInventory.AddItemData(data);
    }
}