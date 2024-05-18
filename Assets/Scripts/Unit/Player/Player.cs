using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
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

    public GameObject furnaceSpawnPoint;
    public GameObject furnace;
    private Inventory inventory;
    private GameObject menuUI;
    private bool MenuUIopen;
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
        MaxHP = GetBaseHP();

        inventory.AddItemData(GameManager.Instance.itemDatas[0]);
    }
    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!MenuUIopen)
            {
                InstantiateFurnace();
                MenuUI.Instance.openMenuUI();
            }
            else
            {
                menuUI.SetActive(false);
            }
            MenuUIopen = !MenuUIopen;
        }
    }
    void FixedUpdate()
    {
        rigid.MovePosition((newPos.normalized * speed * Time.fixedDeltaTime) + rigid.position);
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
    }



//플레이어 스탯 관련
    [Serializable]
    public struct StatLevel
    {
        public int ATKLevel;
        public int DEFLevel;
        public int SpeedLevel;
        public int ATKSpeedLevel;
        public int ATKRangeLevel;
        public int CooldownReductionLevel;
        public int ATKdurationLevel;
        public int GreedLevel;
        public int DelicacyLevel;
        public int WisdomLevel;
        public int TemptationLevel;
        public int StatMaxLevel;
    }
    public StatLevel PlayerStatLevel = new StatLevel();

    
    public float WIsdomDelicacy()
    {
        float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 5);
        float Wisdom = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.WisdomLevel / 5);

        return DelicacyRate * Wisdom;
    }

}
