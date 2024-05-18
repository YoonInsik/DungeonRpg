using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public GameObject furnaceUI;
    public Player player;
    // 범위 안에 있는지
    public bool isIn;
    // Start is called before the first frame update
    void Start()
    {
        player = UnitManager.Instance.player.GetComponent<Player>();
        furnaceUI = FurnaceItemUI.Instance.gameObject;
        isIn = furnaceUI.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            furnaceUI.SetActive(false);
            if (isIn )
            {
                OpenFurnaceUI();
            }
        }
        //if (isIn == true && Input.GetKeyDown(KeyCode.E))
        //{
        //    OpenFurnaceUI();
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == player)
        {
            //Debug.Log(collision.name);
            isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == player)
        {
            isIn = false;
            //furnaceUI.SetActive(false);
        }   
    }

    public void OpenFurnaceUI()
    {
        bool isOpen = furnaceUI.activeSelf;
        furnaceUI.SetActive(!isOpen);
    }
}
