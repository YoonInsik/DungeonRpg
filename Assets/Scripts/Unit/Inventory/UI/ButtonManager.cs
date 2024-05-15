using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public List<CookingItem> cookingItems;
    public FurnaceItemUI FurnaceItemUI;

    // Start is called before the first frame update
    void Start()
    {
        cookingItems = Inventory.Instance.GetCookingList();
        foreach (var item in cookingItems)
        {
            CreateButton(item);
        }

        FurnaceItemUI.gameObject.SetActive(false);
    }

    public void CreateButton(CookingItem item)
    {
        GameObject button = Instantiate(buttonPrefab, transform);

        Image buttonCooking = button.transform.GetChild(0).GetComponent<Image>();
        if (buttonCooking != null) { buttonCooking.sprite = item.GetComponent<SpriteRenderer>().sprite; }
        else { Debug.Log("요리아이템 못찾음"); }

        for(int i = 1; i< 5; i++)
        {
            if(i-1 < item.recipe.Count)
            {
                Image image1 = button.transform.GetChild(i).GetComponent<Image>();
                image1.sprite = item.recipe[i-1].ingredient.icon;

                if (button.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>() != null)
                {
                    TextMeshProUGUI text1 = button.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    text1.text = item.recipe[i - 1].amount.ToString();
                }
            }
            else
            {
                button.transform.GetChild(i).gameObject.SetActive(false);
            }
           
        }

        button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(item));
    }

    public void ButtonClick(CookingItem item)
    {
        FurnaceItemUI.CookingItem(item);
    }
}
