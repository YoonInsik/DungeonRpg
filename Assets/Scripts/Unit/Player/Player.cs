using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
    [Header ("ȭ��") ]

    public GameObject furnaceSpawnPoint;
    public GameObject furnace;

    private Inventory inventory;
    private GameObject menuUI;
    private bool MenuUIopen;

    [Header ("��ĳ��")]
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

    
    [Header("�÷��̾� ���� ����")]
    public StatLevel PlayerStatLevel = new StatLevel();

    //�丮������ ���� ������ ����� �Ǵ� Ÿ�̹� �Ǵ�
    [NonSerialized]
    public bool pause = false;
    
    public float WIsdomDelicacy()
    {
        float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 10);
        float Wisdom = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.WisdomLevel / 5);

        return DelicacyRate * Wisdom;
    }

    public float GreedDelicacy()
    {
        float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 10);
        float Greed = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.GreedLevel / 5);

        return DelicacyRate * Greed;
    }

    //����ü �ӵ�
    public float ATKSpeedDelicacy()
    {
        float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 10);
        float ATKSpeed = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.ATKSpeedLevel / 5);

        return DelicacyRate * ATKSpeed;
    }

    //���� �� �ð�
    public float ATKCooldownDelicacy()
    {
        //3%, 7%�� ��Ÿ�� ����
        float DelicacyRate = 1 - (UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel * 0.03f);
        float CooldownReduction = 1 - (UnitManager.Instance.player.PlayerStatLevel.CooldownReductionLevel * 0.07f);

        return DelicacyRate * CooldownReduction;
    }

    //���ݹ���
    public float ATKRangeDelicacy()
    {
        float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 12);
        float ATKRange = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.ATKRangeLevel / 6);

        return DelicacyRate * ATKRange;
    }
    
    

    //���ӽð�
    //public float ATKDurationDelicacy()
    //{
    //    float DelicacyRate = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.DelicacyLevel / 10);
    //    float statLevel = (1 + (float)UnitManager.Instance.player.PlayerStatLevel.ATKdurationLevel / 5);

    //    return DelicacyRate * statLevel;
    //}

    //�÷��̾� ���� ����
    [Serializable]
    public struct StatLevel
    {
        public int ATKLevel;
        public int DEFLevel;
        public int SpeedLevel;
        public int ATKSpeedLevel;
        public int ATKRangeLevel;
        public int CooldownReductionLevel;
        //public int ATKdurationLevel;
        public int GreedLevel;
        public int DelicacyLevel;
        public int WisdomLevel;
        public int TemptationLevel;
        public int StatMaxLevel;
    }
}
