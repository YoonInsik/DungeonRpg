using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : Singleton<DropManager>
{
    public List<MeatItem> dropItems;

    public DropData GetRandomDropData()
    {
        var item = dropItems[Random.Range(0, dropItems.Count)];
        DropData drop = new DropData(item.name, item, 1.0f);
        return drop;
    }
}
