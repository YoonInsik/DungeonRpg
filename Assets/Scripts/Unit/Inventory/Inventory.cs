using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    //보유 아이템 리스트
    public List<PlayerItemData> itemDataList;

    //현재 갖고있는 요리아이템 저장
    [SerializeField] private Cooking[] cooking;
    [SerializeField] private Meat[] meats;
    //요리로 생성될 아이템 저장

    [SerializeField] private List<CookingItem> cookingList;

    [SerializeField] private List<List<Vector2>> weaponPositionsList;

    public Player player;
    public CookingStatIncrease statUp;
    public AlertText alertText;
    private void Start()
    {
        cooking = new Cooking[3];
        itemDataList = new List<PlayerItemData>();
        statUp = UnitManager.Instance.player.GetComponent<CookingStatIncrease>();
        player = UnitManager.Instance.player.GetComponent<Player>();

        weaponPositionsList = new List<List<Vector2>>();
        weaponPositionsList.Add(new List<Vector2>
        { // one weapon
            new Vector2(0, 0.1f)
        });
        weaponPositionsList.Add(new List<Vector2>
        { // two weapon
            new Vector2(0.5f, 0.1f),
            new Vector2(-0.5f, 0.1f)
        });
        weaponPositionsList.Add(new List<Vector2>
        { // three weapon
            new Vector2(0.5f, 0.1f),
            new Vector2(-0.5f,0.1f),
            new Vector2(0, 1f)
        });
        weaponPositionsList.Add(new List<Vector2>
        { // four weapon
            new Vector2(0.5f, 0.1f),
            new Vector2(-0.5f,0.1f),
            new Vector2(0.5f, 1),
            new Vector2(-0.5f,1)

        });
        weaponPositionsList.Add(new List<Vector2>
        { // five weapon
            new Vector2(0.5f, -0.1f),
            new Vector2(-0.5f, -0.1f),
            new Vector2(0.8f, 0.5f),
            new Vector2(-0.8f, 0.5f),
            new Vector2(0, 1f)
        });
        weaponPositionsList.Add(new List<Vector2>
        { // six weapon
            new Vector2(0.5f, -0.1f),
            new Vector2(-0.5f, -0.1f),
            new Vector2(0.8f, 0.5f),
            new Vector2(-0.8f, 0.5f),
            new Vector2(0.5f, 1f),
            new Vector2(-0.5f, 1f)
        });
    }

    public void AddMeat(MeatItem meat)
    {
        for(int i = 0; i < meats.Length; i++)
        {
            if (meat.itemName.Equals(meats[i].meats.itemName))
            {
                Debug.Log(meat);
                meats[i].count++;
            }
        }
    }

    public void AddCookingItem(CookingItem item)
    {
        int index = cookingList.IndexOf(item);
    
        for (int i = 0; i < cooking.Length; i++)
        {
            if (cooking[i].cooking == null)
            {
                Debug.Log("비었다");
                cooking[i].cooking = cookingList[index];
                cooking[i].count += 1;
                return;
            }
            else if (cooking[i].cooking.name == cookingList[index].name)
            {
                Debug.Log("same name");
                cooking[i].count += 1;
                return;
            }
        }
        alertText.InstantiateAlert("더 이상 아이템을 추가할 수 없습니다.");
    }

    public void RemoveCookingItem(int index)
    {
        if (cooking[index].count == 1)
        {
            cooking[index] = new Cooking(null,0);
        }
        else
        {
            cooking[index] = new Cooking(cooking[index].cooking, cooking[index].count - 1);
        }
    }

    public Cooking[] GetCookingItem()
    {
        return cooking;
    }

    public Meat[] GetMeats()
    {
        return meats;
    }

    public List<CookingItem> GetCookingList()
    {
        return cookingList;
    }

    public void AddItemData(ItemData data)
    {
        if (itemDataList.Count > 6) return;

        var playerItem = itemDataList.Find(x => x.itemData.Equals(data));
        if (playerItem is null)
        {
            playerItem = new PlayerItemData();
            playerItem.itemData = data;
            itemDataList.Add(playerItem);

            foreach (Transform child in player.transform)
            {
                if (child.name.Equals(data.itemName))
                {
                    child.gameObject.SetActive(true);
                    playerItem.itemInstance = child.gameObject.GetComponent<WeaponBase>();
                    RepositionWeapons();
                    print(child.name);
                }
            }
        }
         
        playerItem.level++;
        playerItem.itemInstance.LevelUp();
        // item damage, stat level up
    }

    private void RepositionWeapons()
    {
        var positions = weaponPositionsList[itemDataList.Count - 1];

        for (int index = 0; index < itemDataList.Count; index++)
        {
            itemDataList[index].itemInstance.offset = positions[index];
        }
    }
}

[Serializable]
public struct Meat
{
    public MeatItem meats;
    public int count;
}

[Serializable]
public struct Cooking
{
    public CookingItem cooking;
    public int count;

    public Cooking(CookingItem cooking, int count)
    {
        this.cooking = cooking;
        this.count = count;
    }
}

[Serializable]
public class PlayerItemData
{
    public int level;
    public ItemData itemData;
    public WeaponBase itemInstance;
}
