using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStatIncrease : MonoBehaviour
{
    //[NonSerialized] 
    public Player player;

    // Start is called before the first frame update
    void Start()
    { 
        player = gameObject.GetComponent<Player>();
    }

    //�丮�����ۿ� �ִ� CookingEffect�� ����
    public void IncreaseStat(CookingItem item)
    {   
        StartCoroutine(item.CookingEffect(player));
    }
}
