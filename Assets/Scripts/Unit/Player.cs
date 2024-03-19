using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2 newPos;
    public Rigidbody2D rigid;
    public float speed = 3.0f;
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;
    }
    private void Update()
    {
        newPos.x = Input.GetAxisRaw("Horizontal");
        newPos.y = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        rigid.MovePosition((newPos.normalized * speed * Time.fixedDeltaTime) + rigid.position);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Meat"))
        {
            Debug.Log("gg");
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
