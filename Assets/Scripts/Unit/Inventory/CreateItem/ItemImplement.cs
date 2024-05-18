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
            SetImage();
        }
    }

    public void SetImage()
    {
        image.sprite = item.icon;
    }

    protected override void Contact()
    {
        Inventory.Instance.AddMeat(item);
        MapManager.Instance.CurChunk.droppedObjList.Remove(gameObject);
        ReleaseObject();
    }
}
