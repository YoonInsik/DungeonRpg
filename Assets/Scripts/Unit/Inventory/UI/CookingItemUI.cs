using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class CookingItemUI : Singleton<CookingItemUI>
{
    private Inventory inventory;
    private Cooking[] cookingInventory;
    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.Instance;
        cookingInventory = inventory.GetCookingItem();
        ButtonCookingItemUI();
    }
 
    // ������ �ִ� �丮�� �޾ƿ� UI�� ǥ��
    public void AddCookingItemUI()
    {
        cookingInventory = inventory.GetCookingItem();
        for (int i = 0; i < cookingInventory.Length; i++)
        {
            Image image = gameObject.transform.GetChild(i).GetChild(0).GetComponent<Image>();

            if (cookingInventory[i].cooking == null)
            {
                image.sprite = null;               
            }
            else
            {
                image.sprite = cookingInventory[i].cooking.icon;
            }
        }
    }

    //�ڽ��� ã�� ��ư�� ����
    public void ButtonCookingItemUI()
    {
        for (int i = 0; i < cookingInventory.Length; i++)
        {
            int index = i;
            Button button = gameObject.transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => ButtonClick(cookingInventory[index].cooking, index));
        }
    }

    public void ButtonClick(CookingItem item, int index)
    {
        Debug.Log(item);
        if(item == null) { return; }
        inventory.statUp.StatIncrease(item);
        inventory.RemoveCookingItem(index);
        AddCookingItemUI();
    }
}
