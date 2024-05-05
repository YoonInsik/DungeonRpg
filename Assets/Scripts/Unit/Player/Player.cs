using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2 newPos;
    private Rigidbody2D rigid;
    public float speed = 3.0f;

    [SerializeField] private float fullness;
    public float Fullness {  get { return fullness; } }
    private float maxFullness = 100;
    public float MaxFullness {  get { return maxFullness; } }
    public Inventory GetInventory { get => inventory; }

    private Inventory inventory;
    private GameObject furnaceUI;
    private bool UIopen;
    public Scanner scanner;

    public void Awake()
    {
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
        inventory = Inventory.Instance;
        furnaceUI = FurnaceItemUI.Instance.gameObject;
    }
    private void Update()
    {
        newPos.x = Input.GetAxisRaw("Horizontal");
        newPos.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void IncreaseFullness(float amount)
    {
        if (fullness + amount > maxFullness)
        {
            fullness = maxFullness;
        }
        else
        {
            fullness += amount;
        }
    }

    public IEnumerator DecreaseFullness()
    {
        while(fullness > 0)
        {
            yield return new WaitForSeconds(5.0f);
            fullness -= 5;
            Debug.Log("Decrease Fullness");
        }
    }

    public int GetBaseHP()
    {
        return baseStat.baseHP;
    }
}
