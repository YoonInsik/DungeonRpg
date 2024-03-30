using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceItemUI : Singleton<FurnaceItemUI>
{
    private Inventory inventory;
    private Meat[] meats;

    [SerializeField] private CookingItemUI CookingItemUI;
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
        inventory = Inventory.Instance;
        meats = inventory.GetMeats();
    }

    public void CookingSteak()
    {
        Debug.Log("sdf");
        if (meats[0].count >= 0)
        {
            Debug.Log("addCookingItem");
            meats[0].count -= 0;
            inventory.AddCookingItem(0);
            Debug.Log("아이템추가완료");
            CookingItemUI.AddCookingItemUI();
        } else
        {
            Debug.Log("print");
        }
    }
}
