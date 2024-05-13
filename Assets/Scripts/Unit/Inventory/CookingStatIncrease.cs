using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStatIncrease : MonoBehaviour
{
    public Player player;
    private Cooking[] cookingInventory;
    // Start is called before the first frame update
    void Start()
    { 
        player = gameObject.GetComponent<Player>();
        cookingInventory = Inventory.Instance.GetCookingItem();
    }

    //요리아이템에 있는 CookingEffect를 실행
    public void IncreaseStat(CookingItem item)
    {   
        StartCoroutine(item.CookingEffect(player));
    }
}
