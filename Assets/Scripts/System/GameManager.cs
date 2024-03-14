using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        MapManager.Instance.GenerateMap();
        UnitManager.Instance.SpawnPlayer(MapManager.Instance.floorPositions.First());
    }

}
