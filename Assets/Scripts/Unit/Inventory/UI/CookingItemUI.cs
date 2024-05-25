using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;

//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class CookingItemUI : Singleton<CookingItemUI>
{
    private Inventory inventory;
    private Cooking[] cookingInventory;
    public  Player player;
    public AlertText alert;
    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.Instance;
        player = UnitManager.Instance.player.GetComponent<Player>();
        cookingInventory = inventory.GetCookingItem();
        ButtonCookingItemUI();
    }
 
    // 가지고 있는 요리를 받아와 UI에 표시
    public void AddCookingItemUI()
    {
        cookingInventory = inventory.GetCookingItem();
        for (int i = 0; i < cookingInventory.Length; i++)
        {
            Image image = gameObject.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            TextMeshProUGUI itemText = gameObject.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            if (cookingInventory[i].cooking == null)
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                itemText.text = null;
            }
            else
            {
                image.sprite = cookingInventory[i].cooking.GetComponent<SpriteRenderer>().sprite;
                image.color = new Color(1, 1, 1, 1);
                itemText.text = cookingInventory[i].count.ToString();
            }
        }
    }

    //자식을 찾아 버튼을 연결
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
        Debug.Log("Click" + item);
        if(item == null) { return; }

        if(player.Fullness + item.Fullness <= player.MaxFullness)
        {
            player.IncreaseFullness(item.Fullness);
            inventory.statUp.IncreaseStat(item);
            inventory.RemoveCookingItem(index);
            AddCookingItemUI();
            SoundManager.Instance.PlayCookingEat();
            StatUI.Instance.GetComponent<StatUI>().UpdateCell();
        }
        else
        {
            alert.InstantiateAlert("포만감이 가득찼습니다.");
        }
    }
}
