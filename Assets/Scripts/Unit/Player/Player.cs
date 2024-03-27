using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2 newPos;
    private Rigidbody2D rigid;
    public float speed = 3.0f;

    private Inventory inventory;
    private GameObject furnaceUI;
    private bool UIopen;
    public Scanner scanner;

    public void Awake()
    {
        scanner = GetComponent<Scanner>();
    }
    private void Start()
    { 
        inventory = Inventory.Instance;
        rigid = GetComponent<Rigidbody2D>();
        furnaceUI = FurnaceItemUI.Instance.gameObject;
    }
    private void Update()
    {
        newPos.x = Input.GetAxisRaw("Horizontal");
        newPos.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            furnaceUI.SetActive(!UIopen);
            UIopen = !UIopen;
        }
    }
    void FixedUpdate()
    {
        rigid.MovePosition((newPos.normalized * speed * Time.fixedDeltaTime) + rigid.position);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Meat"))
        {
            MeatItem meat = collision.gameObject.GetComponent<ItemImplement>().item;

            if (meat != null)
            {
                inventory.AddMeat(meat);
                Debug.Log(meat.name);
            }

            Destroy(collision.gameObject);
        }
    }
}
