using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private Player player;

    public void SpawnPlayer(Vector2Int spawnPos)
    {
        var newPlayer = Instantiate(player);
        newPlayer.transform.position = new Vector3(spawnPos.x, spawnPos.y, newPlayer.transform.position.z);
    }
}
