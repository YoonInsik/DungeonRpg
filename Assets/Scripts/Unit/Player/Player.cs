using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2 newPos;
    private Rigidbody2D rigid;
    public float speed = 3.0f;

    [SerializeField] private float fullness = 0;
    public float Fullness {  get { return fullness; } }
    private float maxFullness = 300;
    public float MaxFullness {  get { return maxFullness; } }
  
    private Inventory inventory;
    private GameObject furnaceUI;
    private bool UIopen;
    public Scanner scanner;

    public void Awake()
    {
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        inventory = Inventory.Instance;
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
                IncreaseFullness(5);
                Debug.Log(meat.name);
            }

            Destroy(collision.gameObject);
        }
    }

    public void IncreaseFullness(int amount)
    {
        if (fullness <= maxFullness)
        {
            fullness += amount;
        } else
        {
            Debug.Log("포만도 가득참");
        }
    }
}
