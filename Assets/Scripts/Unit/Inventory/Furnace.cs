using System.Collections;
using System.Collections.Generic;
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
        OpenFurnaceUI();
        CheckDestory();

        e_key.SetActive(isIn);
    }

    [Header("안내창")]
    [SerializeField] GameObject e_key;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E누름");
            if (isIn)
            {
                bool isOpen = furnaceUI.activeSelf;
                furnaceUI.SetActive(!isOpen);
            }
            else
            {
                furnaceUI.SetActive(false);
            }
        }
    }

    // 방을 넘어가면 원래 있던게 사라짐
    public void CheckDestory()
    {
        if(player.pause == false)
        {
            furnaceUI.SetActive(false);
            Destroy(gameObject);
        }
    }
}
