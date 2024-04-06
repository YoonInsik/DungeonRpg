using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceItemUI : Singleton<FurnaceItemUI>
{
    private Inventory inventory;
    private Meat[] meats;
    private List<CookingItem> cookingList;

    [SerializeField] private CookingItemUI CookingItemUI;
    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.Instance;
        meats = inventory.GetMeats();
        cookingList = inventory.GetCookingList();
    }

    public void CookingItem(CookingItem item)
    {
        meats = inventory.GetMeats();

        foreach (var ingredient in item.recipe)
        {
            for (int i = 0; i < meats.Length; i++)
            {
                if (meats[i].meats.itemName == ingredient.ingredient.itemName && meats[i].count < ingredient.amount)
                {
                    Debug.Log(meats[i].meats.itemName + ingredient.ingredient.itemName + meats[i].count + ingredient.amount);
                    return;
                }
            }
        }

        foreach (var ingredient in item.recipe)
        {
            for (int i = 0; i < meats.Length; i++)
            {
                if (meats[i].meats.itemName == ingredient.ingredient.itemName)
                {
                    meats[i].count -= ingredient.amount;
                    inventory.AddCookingItem(item);
                    Debug.Log("아이템추가완료");
                    CookingItemUI.AddCookingItemUI();
                }
            }
        }
    }
}
