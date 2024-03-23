using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingItems : MonoBehaviour
{
    private Inventory inventory;
    private Meat[] meats;

    public CookingItemUI CookingItemUI;
    // Start is called before the first frame update
    void Start()
    { 
        gameObject.SetActive(false);
        inventory = Inventory.instance;
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
