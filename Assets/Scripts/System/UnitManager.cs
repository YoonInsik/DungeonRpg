using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private Player playerPrefab;
    public Player player;

    public void SpawnPlayer(Vector2Int spawnPos)
    {
        var newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.position = new Vector3(spawnPos.x, spawnPos.y, newPlayer.transform.position.z);

        player = newPlayer;
    }
}
