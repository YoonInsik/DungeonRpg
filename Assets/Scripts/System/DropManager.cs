using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : Singleton<DropManager>
{
    public List<GameObject> dropPrefabs;

    public DropData GetRandomDropData()
    {
        var prefab = GetRandomDropPrefab();

        DropData drop = new DropData(prefab.name, prefab, 1.0f);
        return drop;
    }

    public GameObject GetRandomDropPrefab()
    {
        return dropPrefabs[Random.Range(0, dropPrefabs.Count)];
    }
}
