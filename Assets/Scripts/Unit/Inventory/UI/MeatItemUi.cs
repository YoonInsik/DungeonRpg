using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MeatItemUI : Singleton<MeatItemUI> 
{
    private Inventory instance;
    [SerializeField] private GameObject[] panels;
    private Meat[] meats;
    private int childCount;
    // Start is called before the first frame update
    private void Start()
    {
        instance = Inventory.Instance;
        meats = instance.GetMeats();
        childCount = gameObject.transform.childCount;

        panels = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            panels[i] = gameObject.transform.GetChild(i).gameObject;
            panels[i].transform.GetChild(0).GetComponent<Image>().sprite = meats[i].meats.icon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childCount;i++)
        {
            panels[i].transform.GetChild(1).GetComponent<Text>().text = ":  "  + meats[i].count.ToString();
        }
    }
}
