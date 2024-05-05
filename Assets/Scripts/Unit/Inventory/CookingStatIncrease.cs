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

    public void IncreaseStat(CookingItem item)
    {
        StartCoroutine(Buff(item));
    }

    IEnumerator Buff(CookingItem item)
    {
        StatIncrease(item);
        if (item.buffDuration > 0)
        {
            yield return new WaitForSeconds(item.buffDuration);
            StatDecrease(item);
            Debug.Log(item.itemName + item.buffDuration + "√  Buff≥°");
        }
    }

    public void StatIncrease(CookingItem item)
    {
        if (item == null) { Debug.Log("StatUP CookingItem NULL"); }
        player.ATK += item.ATK;
        player.speed += item.SPEED;
        player.HP += item.HP;
        player.DEF += item.DEF;
    }

    public void StatDecrease(CookingItem item)
    {
        if (item == null) { Debug.Log("StatUP CookingItem NULL"); }
        player.ATK -= item.ATK;
        player.speed -= item.SPEED;
        player.HP -= item.HP;
        player.DEF -= item.DEF;
    }
}
