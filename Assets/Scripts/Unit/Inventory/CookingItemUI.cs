using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class CookingItemUI : MonoBehaviour
{
    private Inventory inventory;
    private Image item;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
    }

    public void AddCookingItemUI()
    {
        Cooking[] cookingItem = inventory.getCookingItem();
        for (int i = 0; i < cookingItem.Length; i++)
        {
            Image image = gameObject.transform.GetChild(i).GetChild(0).GetComponent<Image>();

            if (cookingItem[i].cooking == null)
            {
                return;
            }
            else
            {
                image.sprite = cookingItem[i].cooking.icon;
            }
        }
    }
}
