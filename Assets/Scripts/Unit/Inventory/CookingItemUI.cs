using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class CookingItemUI : Singleton<CookingItemUI>
{
    private Inventory inventory;
    private Image item;
    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.Instance;
    }
 
    public void AddCookingItemUI()
    {
        Cooking[] cookingItem = inventory.GetCookingItem();
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
