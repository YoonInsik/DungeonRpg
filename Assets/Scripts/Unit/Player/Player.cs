using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2 newPos;
    private Rigidbody2D rigid;
    [SerializeField] private Animator animator;

    public float speed = 3.0f;
    public int MaxHP;
    [SerializeField] private float fullness;
    public float Fullness {  get => fullness; }
    private float maxFullness = 100;
    public float MaxFullness {  get { return maxFullness; } }
    public int fullnessDecreaseAmount = 1;

    public Inventory GetInventory { get => inventory; }

    //[NonSerialized] 
    public bool isDead = false;

    [Header ("화로") ]

    public GameObject furnaceSpawnPoint;
    public GameObject furnace;

    private Inventory inventory;
    private GameObject menuUI;
    private bool MenuUIopen;

    [Header ("스캐너")]
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

        menuUI = MenuUI.Instance.gameObject;
        MenuUI.Instance.gameObject.SetActive(false);

        MaxHP = GetBaseHP();

        inventory.AddItemData(GameManager.Instance.itemDatas[0]);
        //inventory.AddItemData(GameManager.Instance.itemDatas[1]);
        //inventory.AddItemData(GameManager.Instance.itemDatas[2]);
        //inventory.AddItemData(GameManager.Instance.itemDatas[3]);
        //inventory.AddItemData(GameManager.Instance.itemDatas[4]);
        //inventory.AddItemData(GameManager.Instance.itemDatas[5]);
    }
    private void Update()
    {   
        if(isDead) return;

        newPos.x = Input.GetAxisRaw("Horizontal");
        newPos.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(newPos.x, 0) && Mathf.Approximately(newPos.y, 0))
        {
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isMove", true);
        }
        animator.SetFloat("MoveX", newPos.x);
        animator.SetFloat("MoveY", newPos.y);

        // 메뉴 UI 
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!MenuUIopen)
            {
                MenuUI.Instance.openMenuUI();
            }
            else
            {
                menuUI.SetActive(false);
            }
            MenuUIopen = !MenuUIopen;
        }

        // 스탯 UI 관련
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject statUI = StatUI.Instance.gameObject;
            bool isActive;

            isActive = statUI.activeSelf ? false : true;
            
            statUI.SetActive(isActive);
            StatUI.Instance.GetComponent<StatUI>().UpdateCell();
        }
    }
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            rigid.MovePosition(rigid.position);
            return;
        }

        rigid.MovePosition((newPos.normalized * speed * Time.fixedDeltaTime) + rigid.position);
    }

    public void IncreaseFullness(float amount)
    {
        if (fullness + amount >= maxFullness)
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
            fullness -= fullnessDecreaseAmount;
            Debug.Log("Decrease Fullness");
        }
    }

    public int GetBaseHP()
    {
        return baseStat.baseHP;
    }

    public void InstantiateFurnace()
    {
        Instantiate(furnace, furnaceSpawnPoint.transform.position, Quaternion.identity);
        FurnaceItemUI.Instance.gameObject.SetActive(true);
        furnace.GetComponent<Furnace>().isIn = true;
    }

    
    [Header("플레이어 스탯 레벨")]

    //요리아이템 버프 지속이 멈춰야 되는 타이밍 판단
    [NonSerialized]
    public bool pause = false;
    
    public float WIsdomDelicacy()
    {
        float DelicacyRate = (1 + (float)StatLevels["DelicacyLevel"] / 10);
        float Wisdom = (1 + (float)StatLevels["WisdomLevel"] / 5);

        return DelicacyRate * Wisdom;
    }

    public float GreedDelicacy()
    {
        float DelicacyRate = (1 + (float)StatLevels["DelicacyLevel"] / 10);
        float Greed = (1 + (float)StatLevels["GreedLevel"] / 5);

        return DelicacyRate * Greed;
    }

    //투사체 속도
    public float ATKSpeedDelicacy()
    { 
        float DelicacyRate = (1 + (float)StatLevels["DelicacyLevel"] / 10);
        float ATKSpeed = (1 + (float)StatLevels["ATKSpeedLevel"] / 5);

        return DelicacyRate * ATKSpeed;
    }

    //공격 간 시간
    public float ATKCooldownDelicacy()
    {
        //3%, 7%씩 쿨타임 감소
        float DelicacyRate = 1 - (StatLevels["DelicacyLevel"] * 0.03f);
        float CooldownReduction = 1 - (StatLevels["CooldownReductionLevel"] * 0.07f);

        return DelicacyRate * CooldownReduction;
    }

    //공격범위
    public float ATKRangeDelicacy()
    {
        float DelicacyRate = (1 + (float)StatLevels["DelicacyLevel"] / 12);
        float ATKRange = (1 + (float)StatLevels["ATKRangeLevel"] / 6);

        return DelicacyRate * ATKRange;
    }

    [SerializeField]
    public Dictionary<string, int> StatLevels = new Dictionary<string, int>
    {
       { "ATKLevel", 0 },
       { "DEFLevel", 0 },
       { "SpeedLevel", 0 },
       { "ATKSpeedLevel", 0 },
       { "ATKRangeLevel", 0},
       { "CooldownReductionLevel", 0 },
       { "GreedLevel", 0 },
       { "DelicacyLevel", 0 },
       { "WisdomLevel", 0 },
       { "TemptationLevel", 0 },
       { "StatMaxLevel", 5 }
    };
}
