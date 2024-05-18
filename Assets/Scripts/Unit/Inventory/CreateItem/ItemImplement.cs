using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImplement : BaseItem
{
    public SpriteRenderer image;
    public MeatItem item;
    // Start is called before the first frame update
    void Start()
    {
        if (item != null && image != null)
        {
            image.sprite = item.icon;
        }
    }


    protected override void Contact()
    {
        MeatItem meat = gameObject.GetComponent<ItemImplement>().item;
        if (meat != null)
        {
            Inventory.Instance.AddMeat(meat);
            Destroy(gameObject);
        }
    }
}
