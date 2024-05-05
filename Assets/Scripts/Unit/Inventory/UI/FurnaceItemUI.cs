using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceItemUI : Singleton<FurnaceItemUI>
{
    private Inventory inventory;
    private Meat[] meats;
    private List<CookingItem> cookingList;
    public AlertText alert;
    [SerializeField] private CookingItemUI CookingItemUI;
    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.Instance;
        meats = inventory.GetMeats();
        cookingList = inventory.GetCookingList();
    }

    // 요리재료와 가지고 있는 재료 비교하여 인벤토리 및 UI에 요리 추가
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
                    alert.InstantiateAlert("재료가 부족합니다.");
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
