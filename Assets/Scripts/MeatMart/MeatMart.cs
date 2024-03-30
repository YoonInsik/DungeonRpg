using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatMart : MonoBehaviour
{
    public static MeatMart instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] Meats;


}
