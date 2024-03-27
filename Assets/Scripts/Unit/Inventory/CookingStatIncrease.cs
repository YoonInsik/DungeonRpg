using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStatIncrease : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    { 
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StatIncrease(CookingItem cookingItem)
    {
        if (cookingItem == null) { Debug.Log("StatUP CookingItem NULL"); }
        player.ATK += cookingItem.ATK;
        player.speed += cookingItem.SPEED;
        player.HP += cookingItem.HP;
        player.DEF += cookingItem.DEF;
    }
}
