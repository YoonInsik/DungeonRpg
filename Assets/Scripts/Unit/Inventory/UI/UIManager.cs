using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance = new UIManager();


    [SerializeField] protected Player player;
    [SerializeField] protected Inventory inventory;

    //[SerializeField] protected CookingItemUI cookingItemUI;
    //[SerializeField] protected FullnessUI fullnessUI;
    //[SerializeField] protected FurnaceItemUI furnaceItemUI;
    //[SerializeField] protected HPUI hpUI;
    //[SerializeField] protected MeatItemUI meatItemUI;
    //[SerializeField] protected MenuUI menuUI;
    //[SerializeField] protected SettingUI settingUI;

    [SerializeField] protected Cooking[] cookingInventory;
    [SerializeField] protected Meat[] meats;
    [SerializeField] protected List<CookingItem> cookingList;
    [SerializeField] protected AlertText alertText;

    // Start is called before the first frame update
    void Start()
    {
        player = UnitManager.Instance.player.GetComponent<Player>();
        inventory = Inventory.Instance;
        meats = inventory.GetMeats();
        cookingList = inventory.GetCookingList();
        cookingInventory = inventory.GetCookingItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
